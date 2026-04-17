create user DinhPhuoc identified by 1903

grant create session to DinhPhuoc

grant create table to DinhPhuoc

alter user DinhPhuoc quota 100M on users

create user TTDPHUOC_QLSV123 identified by 1903

grant create session to TTDPHUOC_QLSV123

grant create table to TTDPHUOC_QLSV

alter user TTDPHUOC_QLSV quota 100M on users

create user DoAn_BMCSDL identified by 12345

grant create session to DoAn_BMCSDL

grant create table to DoAn_BMCSDL

alter user DoAn_BMCSDL quota 100M on users

GRANT CONNECT, RESOURCE TO DoAn_BMCSDL;
GRANT CREATE TABLE TO DoAn_BMCSDL;
GRANT CREATE SEQUENCE TO DoAn_BMCSDL;
GRANT UNLIMITED TABLESPACE TO DoAn_BMCSDL;

GRANT SELECT ON DoAn_BMCSDL."ACCOUNT" TO DoAn_BMCSDL;


create user DinhPhuoc_BTT7 identified by 1903

grant create session to DinhPhuoc_BTT7

grant create table to DinhPhuoc_BTT7

alter user DinhPhuoc_BTT7 quota 100M on users

DROP PUBLIC SYNONYM ENCRYPT_ADD;


SELECT USERNAME, ACCOUNT_STATUS, CREATED FROM DBA_USERS;

GRANT CREATE PROCEDURE TO DinhPhuoc_BTT7;
GRANT CREATE FUNCTION TO DinhPhuoc_BTT7;
GRANT CREATE ANY PROCEDURE TO DinhPhuoc_BTT7;

create user CeasarCipher identified by 1903

grant create session to CeasarCipher

grant create table to CeasarCipher

alter user CeasarCipher quota 100M on users

GRANT CREATE PROCEDURE TO CeasarCipher;

CREATE OR REPLACE FUNCTION fn_check_login(p_user IN VARCHAR2)
RETURN NUMBER
AS
BEGIN
    IF UPPER(p_user) IN ('SYS', 'SYSDBA') THEN
        RAISE_APPLICATION_ERROR(-20001, 'User SYS không được phép đăng nhập qua ứng dụng!');
    END IF;

    RETURN 1; -- hợp lệ
EXCEPTION
    WHEN OTHERS THEN
        RETURN 0; -- có lỗi hoặc user bị chặn
END;
/

-- Hàm mã hoá cộng key
CREATE OR REPLACE FUNCTION encrypt_add(p_text IN VARCHAR2, p_key IN NUMBER)
RETURN VARCHAR2
IS
    v_result VARCHAR2(4000) := '';
    v_char CHAR(1);
    v_ascii NUMBER;
    v_encrypted_ascii NUMBER;
BEGIN
    FOR i IN 1 .. LENGTH(p_text) LOOP
        v_char := SUBSTR(p_text, i, 1);
        v_ascii := ASCII(v_char);

        IF v_ascii BETWEEN 32 AND 126 THEN
            v_encrypted_ascii := MOD((v_ascii - 32 + p_key), 95) + 32;
            v_result := v_result || CHR(v_encrypted_ascii);
        ELSE
            v_result := v_result || v_char;
        END IF;
    END LOOP;

    RETURN v_result;
END;
/

GRANT EXECUTE ON ENCRYPT_ADD TO PUBLIC;



CREATE TABLE ACTIVE_SESSIONS (
    USERNAME VARCHAR2(128),
    SESSION_ID VARCHAR2(24),
    SERIAL_NUM VARCHAR2(24),
    LOGIN_TIME DATE DEFAULT SYSDATE,
    CONSTRAINT PK_ACTIVE_SESSIONS PRIMARY KEY (USERNAME, SESSION_ID)
);

GRANT SELECT, INSERT ON ACTIVE_SESSIONS TO DoAn_BMCSDL;
GRANT SELECT ON V_$SESSION TO DoAn_BMCSDL;

GRANT EXECUTE ON ENCRYPT_ADD TO PUBLIC;

CREATE PUBLIC SYNONYM ENCRYPT_ADD FOR SYS.ENCRYPT_ADD;

GRANT EXECUTE ON SYS.ENCRYPT_ADD TO DINHPHUOC_SYS;

SELECT ENCRYPT_ADD('aaa', 19) FROM dual;

ALTER TABLE ACCOUNT
ADD (SESSION_TOKEN VARCHAR2(50) NULL);

DESC ACCOUNT;

-- Lệnh 1: Kiểm tra xem user đăng nhập có sở hữu bảng ACCOUNT nào không
SELECT owner, table_name FROM dba_tables WHERE table_name = 'ACCOUNT' AND owner = 'DoAn_BMCSDL';

-- Lệnh 2: Kiểm tra xem cột SESSION_TOKEN có trong bảng của user đó không
SELECT column_name FROM dba_tab_columns WHERE table_name = 'ACCOUNT' AND owner = 'DoAn_BMCSDL' AND column_name = 'SESSION_TOKEN';

SELECT owner, table_name 
FROM dba_tables 
WHERE table_name = 'ACCOUNT';