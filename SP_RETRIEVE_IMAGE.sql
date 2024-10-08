DROP PROCEDURE IF EXISTS school.SP_RETRIEVE_IMAGE;
CREATE PROCEDURE SP_RETRIEVE_IMAGE (argImgID INT)
BEGIN
  IF(argImgID = 0) THEN
      SELECT 
            IMGID AS ID, IMGNAME AS IMAGE, IMGSIZE AS SIZE 
      FROM gallery 
      ORDER BY IMGID DESC;
  ELSE
	    SELECT 
            ImgData AS IMAGE, IMGNAME AS FILENAME, FILETYPE 
      FROM gallery 
      WHERE ImgID = argImgID 
      ORDER BY IMGID DESC;
  END IF;
END;

