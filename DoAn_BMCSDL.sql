-- Bảng ROLE
CREATE TABLE ROLES (
    ROLE_NAME VARCHAR2(20) PRIMARY KEY
);

INSERT INTO ROLES VALUES ('USER');
INSERT INTO ROLES VALUES ('ADMIN');

-- Bảng ACCOUNT
CREATE TABLE ACCOUNT (
    USERNAME VARCHAR2(30) PRIMARY KEY,
    PASSWORD VARCHAR2(100) NOT NULL,
    ROLE_NAME VARCHAR2(20) DEFAULT 'USER',
    CONSTRAINT fk_account_role FOREIGN KEY (ROLE_NAME) REFERENCES ROLES(ROLE_NAME)
);
select * from account

-- Bảng USERINFO
    CREATE TABLE USERINFO (
        USERNAME VARCHAR2(30) PRIMARY KEY,
        FULLNAME VARCHAR2(100),
        DATEOFBIRTH DATE,
        EMAIL VARCHAR2(100),
        PHONENUMBER VARCHAR2(15),
        CONSTRAINT fk_userinfo_account FOREIGN KEY (USERNAME) REFERENCES ACCOUNT(USERNAME)
    );
select * from userinfo
delete from userinfo
where USERNAME = 'phuc123'
-- Bảng AUDIT_LOG
CREATE TABLE AUDIT_LOG (
    LOG_ID NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    USERNAME VARCHAR2(30),
    ACTION VARCHAR2(50),
    ACTION_DATE TIMESTAMP DEFAULT SYSTIMESTAMP,
    CONSTRAINT fk_audit_account FOREIGN KEY (USERNAME) REFERENCES ACCOUNT(USERNAME)
);

COMMIT;

select * from account
select * from USERINFO
select * from AUDIT_LOG

-- USERINFO
ALTER TABLE USERINFO DROP CONSTRAINT fk_userinfo_account;

ALTER TABLE USERINFO 
ADD CONSTRAINT fk_userinfo_account 
FOREIGN KEY (USERNAME) 
REFERENCES ACCOUNT(USERNAME) ON DELETE CASCADE;

-- AUDIT_LOG
ALTER TABLE AUDIT_LOG DROP CONSTRAINT fk_audit_account;

ALTER TABLE AUDIT_LOG 
ADD CONSTRAINT fk_audit_account 
FOREIGN KEY (USERNAME) 
REFERENCES ACCOUNT(USERNAME) ON DELETE CASCADE;

DELETE FROM ACCOUNT WHERE USERNAME = 'Haha';

CONNECT DOAN_BMCSDL/12345;

CREATE OR REPLACE FUNCTION get_user_tables
RETURN SYS_REFCURSOR
AS
    v_cursor SYS_REFCURSOR;
BEGIN
    OPEN v_cursor FOR
        SELECT TABLE_NAME FROM USER_TABLES ORDER BY TABLE_NAME;
    RETURN v_cursor;
END;
/

CREATE OR REPLACE FUNCTION get_table_data(p_table_name VARCHAR2)
RETURN SYS_REFCURSOR
AS
    v_cursor SYS_REFCURSOR;
    v_sql VARCHAR2(1000);
BEGIN
    v_sql := 'SELECT * FROM ' || p_table_name;
    OPEN v_cursor FOR v_sql;
    RETURN v_cursor;
END;
/

CREATE OR REPLACE FUNCTION encrypt_add(p_plain VARCHAR2, p_key NUMBER)
RETURN VARCHAR2
AS
    v_result VARCHAR2(4000) := '';
    v_alphabet CONSTANT VARCHAR2(100) := 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz01234567';
    v_char CHAR(1);
    v_index NUMBER;
    v_new_index NUMBER;
BEGIN
    FOR i IN 1..LENGTH(p_plain) LOOP
        v_char := SUBSTR(p_plain, i, 1);
        v_index := INSTR(v_alphabet, v_char);
        IF v_index > 0 THEN
            v_new_index := MOD(v_index - 1 + p_key, LENGTH(v_alphabet)) + 1;
            v_result := v_result || SUBSTR(v_alphabet, v_new_index, 1);
        ELSE
            v_result := v_result || v_char;
        END IF;
    END LOOP;
    RETURN v_result;
END;
/

CREATE OR REPLACE FUNCTION encrypt_mul(p_plain VARCHAR2, p_key NUMBER)
RETURN VARCHAR2
AS
    v_result VARCHAR2(4000) := '';
    v_alphabet CONSTANT VARCHAR2(100) := 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz01234567';
    v_char CHAR(1);
    v_index NUMBER;
    v_new_index NUMBER;
BEGIN
    FOR i IN 1..LENGTH(p_plain) LOOP
        v_char := SUBSTR(p_plain, i, 1);
        v_index := INSTR(v_alphabet, v_char);
        IF v_index > 0 THEN
            v_new_index := MOD((v_index - 1) * p_key, LENGTH(v_alphabet)) + 1;
            v_result := v_result || SUBSTR(v_alphabet, v_new_index, 1);
        ELSE
            v_result := v_result || v_char;
        END IF;
    END LOOP;
    RETURN v_result;
END;
/

CREATE OR REPLACE FUNCTION encrypt_random(p_plain VARCHAR2, p_key NUMBER)
RETURN VARCHAR2
AS
    v_choice NUMBER;
    v_result VARCHAR2(4000);
BEGIN
    v_choice := MOD(DBMS_RANDOM.VALUE(0, 2), 2);
    IF v_choice < 1 THEN
        v_result := encrypt_add(p_plain, p_key);
    ELSE
        v_result := encrypt_mul(p_plain, p_key);
    END IF;
    RETURN v_result;
END;
/

ALTER TABLE ACCOUNT
ADD (SESSION_TOKEN VARCHAR2(50) NULL);

                                                                