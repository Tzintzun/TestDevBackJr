

-- Consulta del usuario que más tiempo ha estado logueado.
WITH LogsOrdenados AS (
	SELECT User_id, TipoMov, fecha,
		ROW_NUMBER() OVER (PARTITION BY User_id ORDER BY fecha) AS numeroFila
	FROM ccloglogin
),
logsEmparejados AS (
	SELECT login.User_id, 
			login.fecha AS FechaLogin, 
			logout.fecha AS FechaLogout, 
			DATEDIFF(SECOND, login.fecha, logout.fecha) AS tiempoLog
	FROM LogsOrdenados AS login
	JOIN LogsOrdenados AS logout
	ON login.User_id = logout.User_id
	AND login.numeroFila + 1 = logout.numeroFila
	AND login.TipoMov = 1
	AND logout.TipoMov = 0
),
tiempoTotal AS (
	SELECT 
		User_id,
		SUM(tiempoLog) AS segundosTotales
	FROM logsEmparejados
	GROUP BY User_id
)
SELECT TOP 1 User_id, 
			CONCAT(segundosTotales/86400 ,' dias, ',
				(segundosTotales % 86400)/3600,' horas, ',
				(segundosTotales % 3600)/60 , ' minutos, ', 
				(segundosTotales % 60), ' segundo') AS "Tiempo total"
FROM tiempoTotal
ORDER BY segundosTotales Desc;

--Consulta del usuario que menos tiempo ha estado logueado
WITH LogsOrdenados AS (
	SELECT User_id, TipoMov, fecha,
		ROW_NUMBER() OVER (PARTITION BY User_id ORDER BY fecha) AS numeroFila
	from ccloglogin
),
logsEmparejados AS (
	SELECT login.User_id, 
			login.fecha AS FechaLogin, 
			logout.fecha AS FechaLogout, 
			DATEDIFF(SECOND, login.fecha, logout.fecha) AS tiempoLog
	FROM LogsOrdenados AS login
	JOIN LogsOrdenados AS logout
	ON login.User_id = logout.User_id
	AND login.numeroFila + 1 = logout.numeroFila
	AND login.TipoMov = 1
	AND logout.TipoMov = 0
),
tiempoTotal AS (
	SELECT 
		User_id,
		SUM(tiempoLog) AS segundosTotales
	FROM logsEmparejados
	GROUP BY User_id
)
Select TOP 1 User_id,
			CONCAT(segundosTotales/86400 ,' dias, ',
				(segundosTotales % 86400)/3600,' horas, ',
				(segundosTotales % 3600)/60 , ' minutos, ', 
				(segundosTotales % 60), ' segundo') AS "Tiempo total"
FROM tiempoTotal
ORDER BY segundosTotales ASC;



--Promedio de logueo por mes

SET LANGUAGE Spanish;

WITH LogsOrdenados AS (
	SELECT User_id, TipoMov, fecha,
		ROW_NUMBER() OVER (PARTITION BY User_id ORDER BY fecha) AS numeroFila
	FROM ccloglogin
),
logsEmparejados AS (
	SELECT login.User_id, 
			login.fecha AS FechaLogin, 
			logout.fecha AS FechaLogout, 
			DATEDIFF(SECOND, login.fecha, logout.fecha) AS tiempoLog
	FROM LogsOrdenados AS login
	JOIN LogsOrdenados AS logout
	ON login.User_id = logout.User_id
	AND login.numeroFila + 1 = logout.numeroFila
	AND login.TipoMov = 1
	AND logout.TipoMov = 0
),
promedioMes AS (
	SELECT 
		User_id,
		AVG(tiempoLog) OVER (PARTITION BY User_ID, MONTH(FechaLogin), YEAR(FechaLogin)) AS promedio,
		DATENAME(MONTH,FechaLogin) AS nombreMes, 
		YEAR(FechaLogin) AS anio 
	FROM logsEmparejados
	GROUP BY DATENAME(MONTH,FechaLogin), User_id, tiempoLog,MONTH(FechaLogin), YEAR(FechaLogin)
)
Select  
	CONCAT('Usuario',
		User_id,' en ',
		nombreMes,' ',
		anio,': ',
		promedio/86400 ,' dias, ',
		(promedio % 86400)/3600,' horas, ',
		(promedio % 3600)/60 , ' minutos, ', 
		(promedio % 60), ' segundo') AS "Tiempo total"
FROM promedioMes
GROUP BY nombreMes, anio, promedio, User_id
ORDER BY User_id ASC, nombreMes ASC, anio ASC
