
Create Procedure ActualizaVales_ListaPrecio 
as
Update VALE_SALIDA_DETALLE_VA1
set pro_preman = M.lpr_prelpr,
    pro_precoa = CA.lpr_prelpr,
    pro_precob = CB.lpr_prelpr,
	pro_prepro = PR.lpr_prelpr,
    pro_preada = AD.lpr_prelpr,
	pro_preper = P.lpr_prelpr
From VALE_SALIDA_DETALLE_VA1 a
--Detalle
left join VALE_SALIDA_VAL b on a.cli_codcli = b.cli_codcli and a.val_numval = b.val_numval
--Manguera
left join LIST_PREC_LPR M on a.pro_codman = M.pro_codpro and a.cli_codcli = M.cli_codcli
--Conector a
left join LIST_PREC_LPR CA on a.pro_codcoa = CA.pro_codpro and a.cli_codcli = CA.cli_codcli
--Conector b
left join LIST_PREC_LPR CB on a.pro_codcob = CB.pro_codpro and a.cli_codcli = CB.cli_codcli
--Protector
left join LIST_PREC_LPR PR on a.pro_codpro = PR.pro_codpro and a.cli_codcli = PR.cli_codcli
--Adaptador
left join LIST_PREC_LPR AD on a.pro_codada = AD.pro_codpro and a.cli_codcli = AD.cli_codcli
--Perno
left join LIST_PREC_LPR P on a.pro_codper = P.pro_codpro and a.cli_codcli = P.cli_codcli
Where b.val_fecVal > (Select max(pfc_fecpfc)as Fecha from PERI_FECH_CORT_PFC where pfc_indpfc = 'C')

Update VALE_SALIDA_DETALLE_VA1
Set va1_preuni = ((ISNULL(pro_preman,0)*ISNULL(val_lonval,0))+ISNULL(pro_precoa,0)+ISNULL(pro_precob,0)+(ISNULL(pro_prepro,0)*val_lonval)+ISNULL(pro_preada,0)+ISNULL(pro_preper,0))
From VALE_SALIDA_DETALLE_VA1 a
left join VALE_SALIDA_VAL b on a.cli_codcli = b.cli_codcli and a.val_numval = b.val_numval
Where b.val_fecVal > (select max(pfc_fecpfc) from PERI_FECH_CORT_PFC where pfc_indpfc = 'C')


Update VALE_SALIDA_DETALLE_VA1
Set va1_pretot = ISNULL(va1_preuni,0) * ISNULL(val_canval,0)    
From VALE_SALIDA_DETALLE_VA1 a
left join VALE_SALIDA_VAL b on a.cli_codcli = b.cli_codcli and a.val_numval = b.val_numval
Where b.val_fecVal > (Select max(pfc_fecpfc) from PERI_FECH_CORT_PFC where pfc_indpfc = 'C')