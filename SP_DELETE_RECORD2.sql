DROP PROCEDURE IF EXISTS SCHOOL.SP_DELETE_RECORD2;
CREATE PROCEDURE SP_DELETE_RECORD2(argIMGID LONGTEXT)
BEGIN
    DECLARE iCount INT DEFAULT 0;
    DECLARE iPos INT DEFAULT 0;
    DECLARE iLen INT DEFAULT 0;  
    
    DECLARE strID VARCHAR(50) DEFAULT NULL;  
    
    SET iLen = LENGTH(argIMGID);
    SELECT iLen;
    
    WHILE (INSTR(argIMGID, '|') > 0) DO
        IF(INSTR(argIMGID, '|') != 0) THEN
            SET iPos = INSTR(argIMGID, '|');
            SET strID = TRIM(SUBSTRING(argIMGID, 1, iPos - 1));
            SELECT strID;
        END IF;
        SET argIMGID = TRIM(SUBSTRING(argIMGID, INSTR(argIMGID, '|') + 1));
        SELECT argIMGID;
        
        SET iCount = iCount + 1;
    END WHILE;        
    
    
    /*DELETE 
    FROM school.gallery 
    WHERE   ImgID = argIMGID;*/
END;
