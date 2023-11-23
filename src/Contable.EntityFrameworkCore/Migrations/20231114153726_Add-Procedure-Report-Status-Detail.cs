using Microsoft.EntityFrameworkCore.Migrations;

namespace Contable.Migrations
{
    public partial class AddProcedureReportStatusDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var createProcSql = @"CREATE PROCEDURE [dbo].[report_status_detail]
                                    @CONFLICT INT = NULL
                                    AS
                                    BEGIN		

                                    SET @CONFLICT        = ISNULL(@CONFLICT, 0) 

                                    SELECT T.STATUS, COUNT(DISTINCT T.ID) AS 'COUNT' 
                                    FROM 
                                    (
	                                    SELECT C.ID, LEFT(P.VALUE,CASE WHEN CHARINDEX('/',P.VALUE) = 0 THEN LEN(P.VALUE) ELSE  CHARINDEX('/',P.VALUE) - 1 END) AS STATUS
	                                    FROM APPCOMPROMISES C
	                                    JOIN APPPARAMETER P ON P.ID = C.STATUSID
	                                    JOIN APPCOMPROMISELOCATIONS CL ON CL.COMPROMISEID  = C.ID
	                                    JOIN APPSOCIALCONFLICTLOCATIONS SCL ON SCL.ID = CL.SOCIALCONFLICTLOCATIONID
	                                    JOIN APPRECORDS R ON R.ID = C.RECORDID 
	                                    WHERE 
		                                      SCL.SOCIALCONFLICTID  = CASE WHEN @CONFLICT    > 0 THEN @CONFLICT    ELSE SCL.SOCIALCONFLICTID  END 
                                    ) T
                                    GROUP BY T.STATUS

                                    END";
            migrationBuilder.Sql(createProcSql);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var dropProcSql = "DROP PROC report_status_detail";
            migrationBuilder.Sql(dropProcSql);

        }
    }
}
