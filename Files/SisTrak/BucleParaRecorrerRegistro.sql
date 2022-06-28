/*

UPDATE VALE_SALIDA_DETALLE_VA1_NSS
   SET PRO_PREMAN = (select LP.lpr_prelpr from LIST_PREC_LPR LP where LP.cli_codcli = CLI_CODCLI AND LP.pro_codpro = pro_codman)

WHERE val_fecVal > (select MAX(pfc_fecpfc)  from PERI_FECH_CORT_PFC )
ORDER BY a.val_fecVal

select a.*
FROM VALE_SALIDA_VAL_NSS A 
*/

DECLARE @numero_vale char(5), @codigo_manguera char(20), @precio_manguera numeric(12,2);

DECLARE C_NELIO CURSOR FOR
SELECT b.val_numval,
       --a.val_fecVal,
       b.pro_codman,
       --b.pro_codcoa,
	   --b.pro_codcob,
	   --b.pro_codpro,
	   --b.pro_codada,
	   --b.pro_codper,
	   b.pro_preman
	   --b.pro_precoa,
	   --b.pro_precob,
	   --b.pro_prepro,
	   --b.pro_preada,
	   --b.pro_preper
FROM VALE_SALIDA_VAL_NSS A 
JOIN VALE_SALIDA_DETALLE_VA1_NSS B  ON A.cli_codcli = B.cli_codcli and a.val_numval = b.val_numval
WHERE a.val_fecVal > (select MAX(pfc_fecpfc)  from PERI_FECH_CORT_PFC )
ORDER BY a.val_fecVal;

OPEN C_NELIO;

FETCH NEXT FROM C_NELIO
INTO @numero_vale, @codigo_manguera, @precio_manguera  
  
WHILE @@FETCH_STATUS = 0  
BEGIN  
    PRINT 'Vale     Código                  Precio '  
	PRINT '----------------------------------------'
    
	PRINT @numero_vale + ' -> ' + @codigo_manguera + ' -> ' + cast(@precio_manguera as char(10))
  
	FETCH NEXT FROM C_NELIO 
	INTO @numero_vale, @codigo_manguera, @precio_manguera  

END;
CLOSE C_NELIO;
DEALLOCATE C_NELIO;

	   /*,
	   
	   lis_preman = (select LP.lpr_prelpr from LIST_PREC_LPR LP where LP.cli_codcli = B.CLI_CODCLI AND LP.pro_codpro = b.pro_codman),
	   lis_precoa = (select LP.lpr_prelpr from LIST_PREC_LPR LP where LP.cli_codcli = B.CLI_CODCLI AND LP.pro_codpro = b.pro_codcoa),
	   lis_precob = (select LP.lpr_prelpr from LIST_PREC_LPR LP where LP.cli_codcli = B.CLI_CODCLI AND LP.pro_codpro = b.pro_codcob),
	   lis_prepro = (select LP.lpr_prelpr from LIST_PREC_LPR LP where LP.cli_codcli = B.CLI_CODCLI AND LP.pro_codpro = b.pro_codpro),
	   lis_preada = (select LP.lpr_prelpr from LIST_PREC_LPR LP where LP.cli_codcli = B.CLI_CODCLI AND LP.pro_codpro = b.pro_codada),
	   lis_preper = (select LP.lpr_prelpr from LIST_PREC_LPR LP where LP.cli_codcli = B.CLI_CODCLI AND LP.pro_codpro = b.pro_codper)
    */

/*
where b.pro_preman <> (select LP.lpr_prelpr from LIST_PREC_LPR LP where LP.cli_codcli = B.CLI_CODCLI AND LP.pro_codpro = b.pro_codman)
or	   b.pro_precoa <> (select LP.lpr_prelpr from LIST_PREC_LPR LP where LP.cli_codcli = B.CLI_CODCLI AND LP.pro_codpro = b.pro_codcoa)
or	   b.pro_precob <> (select LP.lpr_prelpr from LIST_PREC_LPR LP where LP.cli_codcli = B.CLI_CODCLI AND LP.pro_codpro = b.pro_codcob)
or	   b.pro_prepro <> (select LP.lpr_prelpr from LIST_PREC_LPR LP where LP.cli_codcli = B.CLI_CODCLI AND LP.pro_codpro = b.pro_codpro)
or	   b.pro_preada <> (select LP.lpr_prelpr from LIST_PREC_LPR LP where LP.cli_codcli = B.CLI_CODCLI AND LP.pro_codpro = b.pro_codada)
or	   b.pro_preper <> (select LP.lpr_prelpr from LIST_PREC_LPR LP where LP.cli_codcli = B.CLI_CODCLI AND LP.pro_codpro = b.pro_codper)
*/
/*
Select pro_codpro, count(pro_codpro) 
from LIST_PREC_LPR 
where cli_codcli = '20508891149'
group by pro_codpro

Select*from VALE_SALIDA_VAL
SELECT* FROM VALE_SALIDA_DETALLE_VA1
Select*from LIST_PREC_LPR
*/

Select * into VALE_SALIDA_VAL_NSS from VALE_SALIDA_VAL
SELECT * into VALE_SALIDA_DETALLE_VA1_NSS FROM VALE_SALIDA_DETALLE_VA1

