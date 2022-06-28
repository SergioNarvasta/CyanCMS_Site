--******************************************************************
--Sentencia que actualiza los valores de la lista de precios
--Probada
--******************************************************************
/*
select m.pro_codpro as lpcodm, 
       m.lpr_prelpr as lpprm,
	   CA.pro_codpro as lpcodcoa, 
       CA.lpr_prelpr as lpprcoa,
	   CB.pro_codpro as lpcodcob, 
       CB.lpr_prelpr as lpprcob,
	   PR.pro_codpro as lpcodpro, 
       PR.lpr_prelpr as lpprpro,
	   AD.pro_codpro as lpcodada, 
       AD.lpr_prelpr as lpprada,
	   P.pro_codpro as lpcodper, 
       P.lpr_prelpr as lpprper,
	   ' -- ' as '-->',
	   a.pro_codman,
	   a.pro_preman,
	   a.pro_codcoa,
	   a.pro_precoa,
	   a.pro_codcob,
	   a.pro_precob,
	   a.pro_codpro,
	   a.pro_prepro,
	   a.pro_codada,
	   a.pro_preada,
	   a.pro_codper,
	   a.pro_preper

*/

update VALE_SALIDA_DETALLE_VA1
set pro_preman = M.lpr_prelpr,
    pro_precoa = CA.lpr_prelpr,
    pro_precob = CB.lpr_prelpr,
	pro_prepro = PR.lpr_prelpr,
    pro_preada = AD.lpr_prelpr,
	pro_preper = P.lpr_prelpr


from VALE_SALIDA_DETALLE_VA1 a

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

where b.val_fecVal > (select max(pfc_fecpfc) from PERI_FECH_CORT_PFC where pfc_indpfc = 'C')

update VALE_SALIDA_DETALLE_VA1
set va1_preuni = ((ISNULL(pro_preman,0)*ISNULL(val_lonval,0))+ISNULL(pro_precoa,0)+ISNULL(pro_precob,0)+(ISNULL(pro_prepro,0)*val_lonval)+ISNULL(pro_preada,0)+ISNULL(pro_preper,0))

--SELECT *
from VALE_SALIDA_DETALLE_VA1 a
left join VALE_SALIDA_VAL b on a.cli_codcli = b.cli_codcli and a.val_numval = b.val_numval
where b.val_fecVal > (select max(pfc_fecpfc) from PERI_FECH_CORT_PFC where pfc_indpfc = 'C')


update VALE_SALIDA_DETALLE_VA1
set va1_pretot = ISNULL(va1_preuni,0) * ISNULL(val_canval,0)    

--SELECT *
from VALE_SALIDA_DETALLE_VA1 a
left join VALE_SALIDA_VAL b on a.cli_codcli = b.cli_codcli and a.val_numval = b.val_numval
where b.val_fecVal > (select max(pfc_fecpfc) from PERI_FECH_CORT_PFC where pfc_indpfc = 'C')

