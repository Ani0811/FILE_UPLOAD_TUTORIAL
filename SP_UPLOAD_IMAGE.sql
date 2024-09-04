DROP PROCEDURE IF EXISTS school.SP_UPLOAD_IMAGE;
CREATE PROCEDURE school.SP_UPLOAD_IMAGE (argImgName VARCHAR(50), argImgSize INT, argImgData LONGBLOB, argFileType VARCHAR(20))
BEGIN
  SET GLOBAL max_allowed_packet = 16777216;
  INSERT INTO SCHOOL.GALLERY
                        (
                          ImgName,
                          ImgSize,
                          ImgData,
                          FileType
                        ) 
  VALUES                (
                          argImgName,
                          argImgSize,
                          argImgData,
                          argFileType
                        );
END;
