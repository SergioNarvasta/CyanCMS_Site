SELECT FechaContrato,
       FechaFactura,
	   FechaVenFactura,
	   FechaEmbProgFin,
	   FechaEmbProgIni,
	   FechaBL,
	   FechaETD,
       FechaETDIni,
	   FechaETA,
	   FechaETAIni,
	   FechaIngAlm,
	   FechaIngAlmEst,
	   FechaIngAlmExt,
	   FechaIngAlmIni,
	   BLFecha,
	   FecIniDescarga,
	   FecFinDescarga,
	   FechaVB,
	   FechaDLVenc,
	   FechaDAM,
	   FechaDAMLevante,
	   FechaInsSucamec,
	   FechaResSucamec,
	   OCFechaApr,
	   OCFechaIng,
	   OCFechaPro 
FROM CEX_Importacion 
WHERE
       YEAR(FechaContrato) <= 1900 OR
       YEAR(FechaFactura) <= 1900 OR   -- YEAR( ) <= 1900 OR 
	   YEAR(FechaVenFactura) <= 1900 OR 
	   YEAR(FechaEmbProgFin) <= 1900 OR 
	   YEAR(FechaEmbProgIni) <= 1900 OR 
	   YEAR(FechaBL) <= 1900 --OR 
	  /* FechaETD IS NULL OR
       FechaETDIni IS NULL OR
	   FechaETA IS NULL OR
	   FechaETAIni IS NULL OR
	   FechaIngAlm IS NULL OR
	   FechaIngAlmEst IS NULL OR
	   FechaIngAlmExt IS NULL OR
	   FechaIngAlmIni IS NULL OR
	   BLFecha IS NULL OR
	   FecIniDescarga IS NULL OR
	   FecFinDescarga IS NULL OR
	   FechaVB IS NULL OR
	   FechaDLVenc IS NULL OR
	   FechaDAM IS NULL OR
	   FechaDAMLevante IS NULL OR
	   FechaInsSucamec IS NULL OR
	   FechaResSucamec IS NULL OR
	   OCFechaApr IS NULL OR
	   OCFechaIng IS NULL OR
	   OCFechaPro IS NULL */
order by Año desc

--UPDATE CEX_Importacion
SET FechaContrato    = Case When YEAR(FechaContrato)   <= 1900 Then NULL Else FechaContrato End

--UPDATE CEX_Importacion
SET FechaFactura     = Case When YEAR(FechaFactura)    <= 1900 Then NULL Else FechaFactura End

--UPDATE CEX_Importacion
SET FechaVenFactura  = Case When YEAR(FechaVenFactura) <= 1900 Then NULL Else FechaVenFactura End

--UPDATE CEX_Importacion
SET FechaEmbProgFin  = Case When YEAR(FechaEmbProgFin) <= 1900 Then NULL Else FechaEmbProgFin End

--UPDATE CEX_Importacion
SET	FechaEmbProgIni  = Case When YEAR(FechaEmbProgIni) <= 1900 Then NULL Else FechaEmbProgIni End

--UPDATE CEX_Importacion
SET FechaBL          = Case When YEAR(FechaBL)         <= 1900 Then NULL Else FechaBL End

--UPDATE CEX_Importacion
SET	FechaETD         = Case When YEAR(FechaETD)        <= 1900 Then NULL Else FechaETD End

--UPDATE CEX_Importacion
SET	FechaETDIni      = Case When YEAR(FechaETDIni)     <= 1900 Then NULL Else FechaETDIni End

--UPDATE CEX_Importacion
SET	FechaETA         = Case When YEAR(FechaETA)        <= 1900 Then NULL Else FechaETA End

--UPDATE CEX_Importacion
SET	FechaETAIni      = Case When YEAR(FechaETAIni)     <= 1900 Then NULL ELse FechaETAIni End

--UPDATE CEX_Importacion
SET   FechaIngAlm      = Case When YEAR(FechaIngAlm)     <= 1900 Then NULL Else FechaIngAlm End

--UPDATE CEX_Importacion
SET	FechaIngAlmEst   = Case When YEAR(FechaIngAlmEst)  <= 1900 Then NULL Else FechaIngAlmEst End

--UPDATE CEX_Importacion
SET	FechaIngAlmExt   = Case When YEAR(FechaIngAlmExt)  <= 1900 Then NULL Else FechaIngAlmExt End

--UPDATE CEX_Importacion
SET	FechaIngAlmIni   = Case When YEAR(FechaIngAlmIni)  <= 1900 Then NULL Else FechaIngAlmIni End

--UPDATE CEX_Importacion
SET   BLFecha          = Case When YEAR(BLFecha)         <= 1900 Then NULL Else BLFecha End

--UPDATE CEX_Importacion
SET	FecIniDescarga   = Case When YEAR(FecIniDescarga)  <= 1900 Then NULL Else FecIniDescarga End

--UPDATE CEX_Importacion
SET	FecFinDescarga   = Case When YEAR(FecFinDescarga)  <= 1900 Then NULL Else FecFinDescarga End

--UPDATE CEX_Importacion
SET	FechaVB          = Case When YEAR(FechaVB)         <= 1900 Then NULL Else FechaVB End

--UPDATE CEX_Importacion
SET	FechaDLVenc      = Case When YEAR(FechaDLVenc)     <= 1900 Then NULL Else FechaDLVenc End

--UPDATE CEX_Importacion
SET   FechaDAM         = Case When YEAR(FechaDAM)        <= 1900 Then NULL Else FechaDAM  End

--UPDATE CEX_Importacion
SET   FechaDAMLevante  = Case When YEAR(FechaDAMLevante) <= 1900 Then NULL Else FechaDAMLevante End

--UPDATE CEX_Importacion
SET   FechaInsSucamec  = Case When YEAR(FechaInsSucamec) <= 1900 Then NULL Else FechaInsSucamec End

--UPDATE CEX_Importacion
SET  FechaResSucamec  = Case When YEAR(FechaResSucamec) <= 1900 Then NULL Else FechaResSucamec  End

--UPDATE CEX_Importacion
SET   OCFechaApr       = Case When YEAR(OCFechaApr)      <= 1900 Then NULL Else OCFechaApr End

--UPDATE CEX_Importacion
SET    OCFechaIng       = Case When YEAR(OCFechaIng)      <= 1900 Then NULL Else OCFechaIng End

--UPDATE CEX_Importacion
SET    OCFechaPro       = Case When YEAR(OCFechaPro)      <= 1900 Then NULL Else OCFechaPro End