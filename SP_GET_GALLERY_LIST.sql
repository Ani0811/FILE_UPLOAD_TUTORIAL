DROP PROCEDURE IF EXISTS SCHOOL.SP_GET_GALLERY_LIST;
CREATE PROCEDURE SCHOOL.SP_GET_GALLERY_LIST (PageIndex int, PageSize int)
BEGIN
  	DECLARE PageLowerBound INT;
  	DECLARE	PageUpperBound INT;
  	DECLARE	RowsToReturn INT;
    DECLARE iRowsCount INT DEFAULT 0;
    DECLARE iCount INT DEFAULT 0;
    
	  SET RowsToReturn = PageSize * (PageIndex + 1);
    
    SET PageLowerBound = ((PageIndex - 1) * PageSize) + 1;
    SET PageUpperBound = ((PageIndex - 1) * PageSize) + PageSize;
    
    DROP TABLE IF EXISTS PageIndex;
  	CREATE TEMPORARY TABLE PageIndex 
  	(
  		IndexId int AUTO_INCREMENT NOT NULL,
  		OrderID int,
      PRIMARY KEY (IndexId)
  	);
    
    DROP TABLE IF EXISTS Details;
    CREATE TEMPORARY TABLE Details 
  	(
  		IndexId int AUTO_INCREMENT NOT NULL,
  		IMGID int,
      IMGNAME varchar(50) DEFAULT NULL,
  		IMGSIZE int,
  		FILETYPE varchar(10) DEFAULT NULL,
      PRIMARY KEY (IndexId)
  	);
    
    /*SET iRowsCount = (SELECT COUNT(*) FROM GALLERY);*/
    /*WHILE iCount < iRowsCount DO*/
        INSERT INTO Details(IMGID, IMGNAME, IMGSIZE, FILETYPE)
        SELECT  IMGID, IMGNAME, IMGSIZE, FILETYPE
        FROM    GALLERY
        ORDER BY IMGID DESC;
        
        /*SET iCount = iCount + 1;*/
    /*END WHILE;*/

    SELECT COUNT(*) AS 'RowsToReturn' FROM Details;
    INSERT INTO PageIndex (OrderID) SELECT IndexId FROM Details;
    
    SELECT  IMGID, IMGNAME, IMGSIZE, FILETYPE
    FROM 
		    (SELECT IndexId, IMGID, IMGNAME, IMGSIZE, FILETYPE
	       FROM Details) AS O,PageIndex AS P 
  	WHERE O.IndexId = P.IndexId
  	AND P.IndexId >= PageLowerBound 
    AND P.IndexId <= PageUpperBound;
    
    DROP TABLE PageIndex;
END;