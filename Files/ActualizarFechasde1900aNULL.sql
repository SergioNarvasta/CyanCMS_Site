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
SET FechaFactura     = Case When YEAR(FechaFactura)    <= 1900 Then NULL End

	FechaVenFactura  = Case When YEAR(FechaVenFactura) <= 1900 Then NULL End,
    FechaEmbProgFin  = Case When YEAR(FechaEmbProgFin) <= 1900 Then NULL End,
	FechaEmbProgIni  = Case When YEAR(FechaEmbProgIni) <= 1900 Then NULL End,
	FechaBL          = Case When YEAR(FechaBL)         <= 1900 Then NULL End,
	FechaETD         = Case When YEAR(FechaETD)        <= 1900 Then NULL End,
	FechaETDIni      = Case When YEAR(FechaETDIni)     <= 1900 Then NULL End,
	FechaETA         = Case When YEAR(FechaETA)        <= 1900 Then NULL End,
	FechaETAIni      = Case When YEAR(FechaETAIni)     <= 1900 Then NULL End,
    FechaIngAlm      = Case When YEAR(FechaIngAlm)     <= 1900 Then NULL End,
	FechaIngAlmEst   = Case When YEAR(FechaIngAlmEst)  <= 1900 Then NULL End,
	FechaIngAlmExt   = Case When YEAR(FechaIngAlmExt)  <= 1900 Then NULL End,
	FechaIngAlmIni   = Case When YEAR(FechaIngAlmIni)  <= 1900 Then NULL End,
    BLFecha          = Case When YEAR(BLFecha)         <= 1900 Then NULL End,
	FecIniDescarga   = Case When YEAR(FecIniDescarga)  <= 1900 Then NULL End,
	FecFinDescarga   = Case When YEAR(FecFinDescarga)  <= 1900 Then NULL End,
	FechaVB          = Case When YEAR(FechaVB)         <= 1900 Then NULL End,
	FechaDLVenc      = Case When YEAR(FechaDLVenc)     <= 1900 Then NULL End,
    FechaDAM         = Case When YEAR(FechaDAM)        <= 1900 Then NULL End,
    FechaDAMLevante  = Case When YEAR(FechaDAMLevante) <= 1900 Then NULL End,
    FechaInsSucamec  = Case When YEAR(FechaInsSucamec) <= 1900 Then NULL End,
    FechaResSucamec  = Case When YEAR(FechaResSucamec) <= 1900 Then NULL End,
    OCFechaApr       = Case When YEAR(OCFechaApr)      <= 1900 Then NULL End,
    OCFechaIng       = Case When YEAR(OCFechaIng)      <= 1900 Then NULL End,
    OCFechaPro       = Case When YEAR(OCFechaPro)      <= 1900 Then NULL End,