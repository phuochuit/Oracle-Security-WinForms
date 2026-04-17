select * from sinhvien

delete from sinhvien
where masv = '112234'

insert into sinhvien values('112234', 'Hung')

-- 2. Thủ tục tạo FGA Động
CREATE OR REPLACE PROCEDURE sp_Add_FGA_Dynamic(
    p_schema IN VARCHAR2,
    p_table  IN VARCHAR2,
    p_policy IN VARCHAR2,
    p_col    IN VARCHAR2
) AS
BEGIN
    DBMS_FGA.ADD_POLICY(
        object_schema   => p_schema,
        object_name     => p_table,
        policy_name     => p_policy,
        audit_column    => p_col,
        enable          => TRUE,
        statement_types => 'SELECT,INSERT,UPDATE,DELETE'
    );
END;
/

-- 3. Thủ tục tạo VPD Động (Tự tạo hàm chính sách + Add Policy)
CREATE OR REPLACE PROCEDURE sp_Add_VPD_Dynamic(
    p_schema    IN VARCHAR2,
    p_table     IN VARCHAR2,
    p_policy    IN VARCHAR2,
    p_condition IN VARCHAR2
) AS
    v_func_name VARCHAR2(50) := 'FUNC_' || p_policy;
    v_sql       VARCHAR2(4000);
BEGIN
    -- Tạo hàm chính sách động trả về điều kiện Where
    v_sql := 'CREATE OR REPLACE FUNCTION ' || p_schema || '.' || v_func_name || 
             '(p_sch IN VARCHAR2, p_obj IN VARCHAR2) RETURN VARCHAR2 AS ' ||
             'BEGIN RETURN ''' || p_condition || '''; END;';
    EXECUTE IMMEDIATE v_sql;

    -- Add Policy VPD
    DBMS_RLS.ADD_POLICY(
        object_schema   => p_schema,
        object_name     => p_table,
        policy_name     => p_policy,
        function_schema => p_schema,
        policy_function => v_func_name,
        statement_types => 'SELECT'
    );
END;
/

-- 4. Thủ tục Flashback (Phục hồi dữ liệu) Động
CREATE OR REPLACE PROCEDURE sp_Flashback_Dynamic(
    p_schema  IN VARCHAR2,
    p_table   IN VARCHAR2,
    p_minutes IN NUMBER
)
AUTHID CURRENT_USER  -- <<== QUAN TRỌNG: Thêm dòng này vào
AS
    v_full_name VARCHAR2(100) := p_schema || '.' || p_table;
    v_sql_alter VARCHAR2(200);
    v_sql_flash VARCHAR2(500);
BEGIN
    -- 1. Bật Row Movement (Bắt buộc cho Flashback)
    -- Dùng Dynamic SQL để nối chuỗi an toàn
    v_sql_alter := 'ALTER TABLE ' || v_full_name || ' ENABLE ROW MOVEMENT';
    EXECUTE IMMEDIATE v_sql_alter;
    
    -- 2. Thực hiện Flashback
    v_sql_flash := 'FLASHBACK TABLE ' || v_full_name || 
                   ' TO TIMESTAMP (SYSTIMESTAMP - INTERVAL ''' || p_minutes || ''' MINUTE)';
    EXECUTE IMMEDIATE v_sql_flash;
    
EXCEPTION
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20001, 'Lỗi Flashback: ' || SQLERRM);
END;
/

-- Thủ tục xóa chính sách VPD động
CREATE OR REPLACE PROCEDURE sp_Drop_VPD_Dynamic(
    p_schema IN VARCHAR2,
    p_table  IN VARCHAR2,
    p_policy IN VARCHAR2
) AS
BEGIN
    -- Gọi gói hệ thống để gỡ bỏ chính sách
    DBMS_RLS.DROP_POLICY(
        object_schema => p_schema,
        object_name   => p_table,
        policy_name   => p_policy
    );
END;
/