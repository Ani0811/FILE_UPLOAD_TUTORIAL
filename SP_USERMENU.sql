DROP PROCEDURE IF EXISTS school.SP_USERMENU;
CREATE PROCEDURE school.SP_USERMENU()
BEGIN
	SELECT MENU_ID, MENU_PARENTID, MENU_DESC, MENU_URL, MENU_LEVEL
  FROM USERMENU
  ORDER BY MENU_LEVEL;
  
  /*WHERE MENU_ISACTIVE = 1
  AND MENU_LEVEL = 0;

  SELECT MENU_ID, MENU_PARENTID, MENU_DESC, MENU_URL, MENU_LEVEL
  FROM USERMENU
  WHERE MENU_PARENTID = TRIM('MENU_HOME')
  AND MENU_ISACTIVE = 1
  /*ORDER BY MENU_ORDER;
  UNION
  SELECT MENU_ID, MENU_PARENTID, MENU_DESC, MENU_URL, MENU_LEVEL
  FROM USERMENU
  WHERE MENU_PARENTID = TRIM('MENU_HELP')
  AND MENU_ISACTIVE = 1
  ORDER BY MENU_LEVEL;
  
  SELECT MENU_ID, MENU_PARENTID, MENU_DESC, MENU_URL, MENU_LEVEL
  FROM USERMENU
  WHERE MENU_PARENTID = TRIM('MENU_MANAGEFILES')
  AND MENU_ISACTIVE = 1
  UNION
  SELECT MENU_ID, MENU_PARENTID, MENU_DESC, MENU_URL, MENU_LEVEL
  FROM USERMENU
  WHERE MENU_PARENTID = TRIM('MENU_FILERETRIEVE')
  AND MENU_ISACTIVE = 1
  ORDER BY MENU_LEVEL;
  
  SELECT MENU_ID, MENU_PARENTID, MENU_DESC, MENU_URL, MENU_LEVEL
  FROM USERMENU
  WHERE MENU_PARENTID = TRIM('MENU_VIEWDATA')
  AND MENU_ISACTIVE = 1
  ORDER BY MENU_LEVEL;*/
END;
