DROP PROCEDURE IF EXISTS SCHOOL.SP_DELETE_RECORD3;
CREATE PROCEDURE SP_DELETE_RECORD3(argIMGID LONGTEXT)
BEGIN
    DECLARE strVal VARCHAR(50) DEFAULT NULL;
    WHILE (LOCATE('|', argIMGID) > 0) DO
        IF(LOCATE('|', argIMGID) != 0) THEN
            SET @V_DESIGNATION = SUBSTRING(argIMGID,1, LOCATE('|',argIMGID) - 1); 
            SET strVal = @V_DESIGNATION;
            /*SELECT strVal;*/
            DELETE 
            FROM school.gallery 
            WHERE ImgID = CONVERT(strVal, SIGNED);
            
        END IF;
        SET argIMGID = SUBSTRING(argIMGID, LOCATE('|',argIMGID) + 1); 
        IF(LOCATE('|', argIMGID) = 0) THEN
            SET strVal = argIMGID;
            /*SELECT strVal;*/
            DELETE 
            FROM school.gallery 
            WHERE ImgID = CONVERT(strVal, SIGNED);
        END IF;
    END WHILE;
END;
