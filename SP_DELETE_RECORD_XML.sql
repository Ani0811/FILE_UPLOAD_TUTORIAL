DROP PROCEDURE IF EXISTS SCHOOL.SP_DELETE_RECORD_XML;
CREATE PROCEDURE SP_DELETE_RECORD_XML(argIMGID_xml LONGTEXT)
BEGIN
    DECLARE iCount INT DEFAULT 1;
    DECLARE iRowCount INT DEFAULT 0;
    DECLARE strVal VARCHAR(50) DEFAULT NULL;
    
    SET iRowCount  = EXTRACTVALUE(argIMGID_xml, 'COUNT(//GALLERY//RECORDS)');
    WHILE iCount <= iRowCount DO
        SET strVal = EXTRACTVALUE(argIMGID_xml, '//RECORDS[$iCount]//IMGID');
        /*SELECT strVal;*/
        DELETE 
        FROM school.gallery 
        WHERE ImgID = CONVERT(strVal, SIGNED);
        SET iCount = iCount + 1;
    END WHILE;
END;
