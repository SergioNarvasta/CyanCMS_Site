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
Where YEAR(FechaContrato)   <= 1900

--UPDATE CEX_Importacion
SET FechaFactura     = Case When YEAR(FechaFactura)    <= 1900 Then NULL Else FechaFactura End
Where YEAR(FechaFactura)   <= 1900

--UPDATE CEX_Importacion
SET FechaVenFactura  = Case When YEAR(FechaVenFactura) <= 1900 Then NULL Else FechaVenFactura End
Where YEAR(FechaVenFactura)   <= 1900

--UPDATE CEX_Importacion
SET FechaEmbProgFin  = Case When YEAR(FechaEmbProgFin) <= 1900 Then NULL Else FechaEmbProgFin End
Where YEAR(FechaEmbProgFin)   <= 1900

--UPDATE CEX_Importacion
SET	FechaEmbProgIni  = Case When YEAR(FechaEmbProgIni) <= 1900 Then NULL Else FechaEmbProgIni End
Where YEAR(FechaEmbProgIni)   <= 1900

--UPDATE CEX_Importacion
SET FechaBL          = Case When YEAR(FechaBL)         <= 1900 Then NULL Else FechaBL End
Where YEAR(FechaBL)   <= 1900}

--UPDATE CEX_Importacion
SET	FechaETD         = Case When YEAR(FechaETD)        <= 1900 Then NULL Else FechaETD End
Where YEAR(FechaETD)   <= 1900

--UPDATE CEX_Importacion
SET	FechaETDIni      = Case When YEAR(FechaETDIni)     <= 1900 Then NULL Else FechaETDIni End
Where YEAR(FechaETDIni)   <= 1900

--UPDATE CEX_Importacion
SET	FechaETA         = Case When YEAR(FechaETA)        <= 1900 Then NULL Else FechaETA End
Where YEAR(FechaETA)   <= 1900

--UPDATE CEX_Importacion
SET	FechaETAIni      = Case When YEAR(FechaETAIni)     <= 1900 Then NULL ELse FechaETAIni End
Where YEAR(FechaETAIni)   <= 1900

--UPDATE CEX_Importacion
SET   FechaIngAlm      = Case When YEAR(FechaIngAlm)     <= 1900 Then NULL Else FechaIngAlm End
Where YEAR(FechaIngAlm)   <= 1900

--UPDATE CEX_Importacion
SET	FechaIngAlmEst   = Case When YEAR(FechaIngAlmEst)  <= 1900 Then NULL Else FechaIngAlmEst End
Where YEAR(FechaIngAlmEst)   <= 1900

--UPDATE CEX_Importacion
SET	FechaIngAlmExt   = Case When YEAR(FechaIngAlmExt)  <= 1900 Then NULL Else FechaIngAlmExt End
Where YEAR(FechaIngAlmExt)   <= 1900

--UPDATE CEX_Importacion
SET	FechaIngAlmIni   = Case When YEAR(FechaIngAlmIni)  <= 1900 Then NULL Else FechaIngAlmIni End
Where YEAR(FechaIngAlmIni)   <= 1900

--UPDATE CEX_Importacion
SET   BLFecha          = Case When YEAR(BLFecha)         <= 1900 Then NULL Else BLFecha End
Where YEAR(BLFecha)   <= 1900

--UPDATE CEX_Importacion
SET	FecIniDescarga   = Case When YEAR(FecIniDescarga)  <= 1900 Then NULL Else FecIniDescarga End
Where YEAR(FecIniDescarga)   <= 1900

--UPDATE CEX_Importacion
SET	FecFinDescarga   = Case When YEAR(FecFinDescarga)  <= 1900 Then NULL Else FecFinDescarga End
Where YEAR(FecFinDescarga)   <= 1900

--UPDATE CEX_Importacion
SET	FechaVB          = Case When YEAR(FechaVB)         <= 1900 Then NULL Else FechaVB End
Where YEAR(FechaVB)   <= 1900

--UPDATE CEX_Importacion
SET	FechaDLVenc      = Case When YEAR(FechaDLVenc)     <= 1900 Then NULL Else FechaDLVenc End
Where YEAR(FechaDLVenc)   <= 1900

--UPDATE CEX_Importacion
SET   FechaDAM         = Case When YEAR(FechaDAM)        <= 1900 Then NULL Else FechaDAM  End
Where YEAR(FechaDAM)   <= 1900

--UPDATE CEX_Importacion
SET   FechaDAMLevante  = Case When YEAR(FechaDAMLevante) <= 1900 Then NULL Else FechaDAMLevante End
Where YEAR(FechaDAMLevante)   <= 1900

--UPDATE CEX_Importacion
SET   FechaInsSucamec  = Case When YEAR(FechaInsSucamec) <= 1900 Then NULL Else FechaInsSucamec End
Where YEAR(FechaInsSucamec)   <= 1900

--UPDATE CEX_Importacion
SET  FechaResSucamec  = Case When YEAR(FechaResSucamec) <= 1900 Then NULL Else FechaResSucamec  End
Where YEAR(FechaContrato)   <= 1900

--UPDATE CEX_Importacion
SET   OCFechaApr       = Case When YEAR(OCFechaApr)      <= 1900 Then NULL Else OCFechaApr End
Where YEAR(FechaContrato)   <= 1900

--UPDATE CEX_Importacion
SET    OCFechaIng       = Case When YEAR(OCFechaIng)      <= 1900 Then NULL Else OCFechaIng End
Where YEAR(FechaContrato)   <= 1900

--UPDATE CEX_Importacion
SET    OCFechaPro       = Case When YEAR(OCFechaPro)      <= 1900 Then NULL Else OCFechaPro End
Where YEAR(FechaContrato)   <= 1900