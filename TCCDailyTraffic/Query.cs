using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TCCDailyTraffic
{
    public class Query
    {
        public static string query;
        public static string getQuery(string sDate , string eDate)
        {
           return query = "if object_id('tempdb..#timeframetable') is not null drop table #timeframetable" +
            " if object_id('tempdb..#deleted') is not null drop table #deleted" +
            " if object_id('tempdb..#reprocessed') is not null drop table #reprocessed" +
            " DECLARE @from smalldatetime = '"+ sDate + "'," +
            " @to smalldatetime = '"+ eDate + "', @companyid int = 93" +
            " declare @monitoringlist table (rn int,storemonitoringid varchar(max),monitoringfordate varchar(max),StoreNumber varchar(max),starttime varchar(max),endtime varchar(max))" +
            " declare @storenumber varchar(max)" +
            " declare @activitydate varchar(max)" +
            " declare @activitytime varchar(max)" +
            " declare @xfrom varchar(max)" +
            " declare @xto varchar(max)" +
            " declare @Xquery nvarchar(max)" +
            " declare @Xloop int = 1" +
            " create table #timeframetable (StoreNumber varchar(max)," +
            " ActivityDate varchar(max),ActivityTime varchar(max),TrafficIn" +
            " int not null default(0),TrafficOut int not null default(0)," +
            " TrafficGroupIn int not null default(0),TrafficGroupOut int not null default(0),Filters int not null default(0))" +
            " Select St.Storeid,St.storeName as RebizStorename,iqclerk.GLCode as" +
            " RebizGLCode, ''''+Convert(varchar(10),gl.GLCode ) +''''as RQDCGLCode,Abbreviation" +
            " into #Stores from Store st" +
            " Inner join centralhub.dbo.BasecampRQ_StoreMapping stmp on stmp.StoreId = st.StoreID and stmp.companyId = 93" +
            " inner join iQclerk_Stores iqclerk on iqclerk.StoreID = stmp.RIQStoreId" +
            " left join RQDC_LocationMaster gl on gl.StoreID = iqclerk.RQDCStoreid" +
            " if object_id('tempdb..#storemonitoring') is not null drop table #storemonitoring" +
            " select * into #storemonitoring from (" +
            " select storeMonitoringID,storeID," +
            " agentID,monitoringForDate,monitoredOn,reviewedBy," +
            " reviewedOn,businessStartTime,businessEndTime,monitoringMode," +
            " reviewComments,monitoringInterval,intervalTypeID,monitoringStatusID," +
            " rejectCount,CONVERT(DATETIME,CONVERT(VARCHAR(13),processForConvDate ,120)+ ':00') " +
            " as processForConvDate from storemonitoring where monitoringfordate > '2019-09-30' AND MonitoringStatusID = 220" +
            " union all" +
            " select storeMonitoringID,storeID,agentID,monitoringForDate,monitoredOn,reviewedBy," +
            " reviewedOn,businessStartTime,businessEndTime,monitoringMode,reviewComments,monitoringInterval," +
            " intervalTypeID,monitoringStatusID,rejectCount," +
            " CONVERT(DATETIME,CONVERT(VARCHAR(13),processForConvDate ,120)+ ':00')" +
            " as processForConvDate from storemonitoring_history where monitoringfordate > '2019-09-30' AND MonitoringStatusID = 220" +
            " ) a" +
            " Update #stores set Abbreviation = 10419 where RebizStoreName = '419 South Bend IN'" +
            " Update #stores set Abbreviation = 10461 where RebizStoreName = '461 Centerville OH'" +
            " Update #stores set Abbreviation = 10570 where RebizStoreName = '0570 New Lenox'" +
            " Update #stores set Abbreviation = 10761 where RebizStoreName = '761 Mt. Vernon IL'" +
            " Update #stores set Abbreviation = 10950 where RebizStoreName = '950 Hudson NY'" +
            " Update #stores set Abbreviation = 10937 where RebizStoreName = '937 Portage IN'" +
            " Update #stores set Abbreviation = 10819 where RebizStoreName = '0819 Hummelstown PA'" +
            " Update #stores set Abbreviation = 10997 where RebizStoreName = '0997 Westland MI'" +
            " Update #stores set Abbreviation = 17043 where RebizStoreName = '7043 Amelia Courthouse VA TCC'" +
            " Update #stores set Abbreviation = 10944 where RebizStoreName = '0944 Orchard Park NY'" +
            " Update #stores set Abbreviation = 14304 where RebizStoreName = '4304 Steubenville OH - (Mall)'" +
            " Update #stores set Abbreviation = 10799 where RebizStoreName = '0799 Palos Park IL'" +
            " Update #stores set Abbreviation = 10888 where RebizStoreName = '0888 Cordova TN'" +
            " Update #stores set Abbreviation = 14602 where RebizStoreName like '%4602%'" +
            " Update #stores set Abbreviation = 14601 where RebizStoreName = '4601 Alpharetta GA'" +
            " Update #stores set Abbreviation = 14682 where RebizStoreName = '4682 White Bear Lake MN'" +
            " Update #stores set Abbreviation = 14767 where RebizStoreName = '4767 Smithtown NY'" +
            " Update #stores set Abbreviation = 14708 where RebizStoreName = '4708 Millville NJ'" +
            " Update #stores set Abbreviation = 14765 where RebizStoreName = '4765 Farmingdale NY'" +
            " Update #stores set Abbreviation = 14691 where RebizStoreName = '4691 Ocean City NJ'" +
            " Update #stores set Abbreviation = 14702 where RebizStoreName = '4702 Waretown NJ'" +
            " Update #stores set Abbreviation = 14710 where RebizStoreName = '4710 Vineland NJ'" +
            " Update #stores set Abbreviation = 14744 where RebizStoreName = '4744 Scranton PA'" +
            " Update #stores set Abbreviation = 14748 where RebizStoreName = '4748 Colmar PA'" +
            " Update #stores set Abbreviation = 14752 where RebizStoreName = '4752 Hamlin PA'" +
            " Update #stores set Abbreviation = 14760 where RebizStoreName = '4760 Newport News VA - (Mall)'" +
            " Update #stores set Abbreviation = 14763 where RebizStoreName = '4763 Greenbrier Pkwy Chesapeake VA (Mall)'" +
            " Update #stores set Abbreviation = 14764 where RebizStoreName = '4764 Oyster Bay NY'" +
            " Update #stores set Abbreviation = 14781 where RebizStoreName = '4781 Tempe AZ (Arizona Mills)'" +
            " Update #stores set Abbreviation = 14785 where RebizStoreName = '4785 Chandler AZ - (Mall)'" +
            " Update #stores set Abbreviation = 14787 where RebizStoreName = '4787 Albuquerque NM (Cottonwood) (Mall)'" +
            " Update #stores set Abbreviation = 14789 where RebizStoreName = '4789 Phoenix AZ (Desert Sky) (Mall)'" +
            " Update #stores set Abbreviation = 14793 where RebizStoreName = '4793 Tucson AZ - (Mall)'" +
            " Update #stores set Abbreviation = 14797 where RebizStoreName = '4797 Phoenix AZ (Metrocenter) (Mall)'" +
            " Update #stores set Abbreviation = 14798 where RebizStoreName = '4798 Phoenix AZ (Paradise Valley) (Mall)'" +
            " Update #stores set Abbreviation = 14799 where RebizStoreName = '4799 Tucson AZ (Park Place 102) (Mall)'" +
            " Update #stores set Abbreviation = 14803 where RebizStoreName = '4803 Prescott AZ - (Mall)'" +
            " Update #stores set Abbreviation = 14810 where RebizStoreName = '4810 Santa Fe NM - (Mall)'" +
            " Update #stores set Abbreviation = 14813 where RebizStoreName = '4813 Mesa AZ (Superstition Springs) (Mall)'" +
            " Update #stores set Abbreviation = 1949 where RebizStoreName = '949 Johnston RI'" +
            " Update #stores set Abbreviation = 14829 where RebizStoreName = '4829 East Stroudsburg PA'" +
            " Update #stores set Abbreviation = 14836 where RebizStoreName = '4836 Winnetka IL'" +
            " Update #stores set Abbreviation = 14837 where RebizStoreName = '4837 Highland Park IL'" +
            " Update #stores set Abbreviation = 14840 where RebizStoreName = '4840 Lakeland FL'" +
            " Update #stores set Abbreviation = 14841 where RebizStoreName = '4841 Daytona Beach FL'" +
            " Update #stores set Abbreviation = 14843 where RebizStoreName = '4843 Vero Beach FL (Indian River Mall) (Mall)'" +
            " Update #stores set Abbreviation = 14845 where RebizStoreName = '4845 Melbourne FL (Mall)'" +
            " Update #stores set Abbreviation = 14824 where RebizStoreName = '4824 Farmingdale NY Broadhollow Rd'" +
            " Update #stores set Abbreviation = 14857 where RebizStoreName = '4857 Portland ME'" +
            " Update #stores set Abbreviation = 14859 where RebizStoreName = '4859 Newington CT Fenn Road'" +
            " Update #stores set Abbreviation = 14872 where RebizStoreName = '4872 Huntley'" +
            " Update #stores set Abbreviation = 14873 where RebizStoreName = '4873 Carpentersville IL'" +
            " Update #stores set Abbreviation = 14875 where RebizStoreName = '4875 Schaumburg IL'" +
            " Update #stores set Abbreviation = 14880 where RebizStoreName = '4880 Kenner LA'" +
            " Update #stores set Abbreviation = 14847 where RebizStoreName = '4847 Tannersville PA (Mall)'" +
            " Update #stores set Abbreviation = 14896 where RebizStoreName = '4896 Cranberry Township PA Freedom'" +
            " Update #stores set Abbreviation = 14904 where RebizStoreName = '4904 Pittsburgh PA Ross Park (Mall)'" +
            " Update #stores set Abbreviation = 14907 where RebizStoreName = '4907 Bethel Park PA'" +
            " Update #stores set Abbreviation = 14953 where RebizStoreName = '4953 Tallahassee FL'" +
            " select StoreNumber,ActivityDate,ActivityTime,sum(TrafficIn) TrafficIn,	sum(TrafficOut) TrafficOut, sum(TrafficGroupIn) TrafficGroupIn, sum(TrafficGroupOut) TrafficGroupOut, sum(TrafficIn+TrafficOut+TrafficGroupIn+TrafficGroupOut) filters into #trafficdata1 from (" +
            " select StoreNumber,monitoringfordate as ActivityDate,[15min] as ActivityTime,isnull((case when action= 'in' then sum(customercount) end),0) as TrafficIn, isnull((case when action='out' then sum(customercount) end),0) as TrafficOut, isnull((case when action= 'in' then count(groupcustomerid) end),0) as TrafficGroupIn, isnull((case when action='out' then count(groupcustomerid) end),0) as TrafficGroupOut from (" +
            " select monitoringfordate,cast(dateadd(minute,(datediff(minute,0,timeframe)/15)*15,0) as time) [15min],storenumber,storeID,groupcustomerid,sum(customercount) customercount,timeframe,action from (" +
            " select st.abbreviation as storenumber,sm.storeid,sm.monitoringfordate," +
            " sm.storemonitoringid," +
            " sg.groupcustomerid,sum(sg.customercount) customercount, ec.timeframe,ec.action" +
            " from " +
            " #storemonitoring sm" +
            " inner join storegroupcustomer sg on sm.storemonitoringid = sg.storemonitoringid" +
            " inner join employeecustomer ec on sg.groupcustomerid = ec.groupcustomerid" +
            " inner join #stores st on sm.storeID = st.storeid" +
            " where cast(sm.processForConvDate as datetime) between @from and @to " +
            " and cast(sm.monitoringfordate as date) > '2019-09-30' and cast(sm.monitoringfordate as date) <= cast(dateadd(day,-2,cast(@to as date)) as date)" +
            " and ec.action in('in','out') and ec.isReEnter = 0 " +
            " group by" +
            " st.abbreviation," +
            " sm.monitoringfordate," +
            " sm.storemonitoringid," +
            " sm.storeID " +
            " ,sg.groupcustomerid, ec.timeframe,ec.action " +
            " ) a " +
            " group by dateadd(minute,(datediff(minute,0,timeframe)/15)*15,0)," +
            " monitoringForDate," +
            " storeid,timeframe,action,groupCustomerID ,storenumber " +
            " ) x" +
            " group by StoreNumber,monitoringfordate,[15min],action" +
            " ) z group by StoreNumber,ActivityDate,ActivityTime order by 1,2,3" +
            " select StoreNumber,ActivityDate,ActivityTime,sum(TrafficIn) TrafficIn,	sum(TrafficOut) TrafficOut, sum(TrafficGroupIn) TrafficGroupIn, sum(TrafficGroupOut) TrafficGroupOut, sum(TrafficIn+TrafficOut+TrafficGroupIn+TrafficGroupOut) Filters into #trafficdata2 from (" +
            " select StoreNumber,monitoringfordate as ActivityDate,[15min] as ActivityTime,isnull((case when action= 'in' then sum(customercount) end),0) as TrafficIn, isnull((case when action='out' then sum(customercount) end),0) as TrafficOut, isnull((case when action= 'in' then count(groupcustomerid) end),0) as TrafficGroupIn, isnull((case when action='out' then count(groupcustomerid) end),0) as TrafficGroupOut from (" +
            " select monitoringfordate,cast(dateadd(minute,(datediff(minute,0,timeframe)/15)*15,0) as time) [15min],storenumber,storeID,groupcustomerid,sum(customercount) customercount,timeframe,action from (" +
            " select st.abbreviation as storenumber,sm.storeid,sm.monitoringfordate," +
            " sm.storemonitoringid," +
            " sg.groupcustomerid,sum(sg.customercount) customercount, ec.timeframe,ec.action" +
            " from " +
            " #storemonitoring sm" +
            " inner join storegroupcustomer sg on sm.storemonitoringid = sg.storemonitoringid" +
            " inner join employeecustomer ec on sg.groupcustomerid = ec.groupcustomerid" +
            " inner join #stores st on sm.storeID = st.storeid" +
            " where cast(sm.processForConvDate as datetime) between dateadd(day,-1,@from) and dateadd(day,-1,@to)" +
            " and cast(sm.monitoringfordate as date) = dateadd(day,-2,cast(@to as date))" +
            " and ec.action in('in','out') and ec.isReEnter = 0" +
            " group by" +
            " st.abbreviation," +
            " sm.monitoringfordate," +
            " sm.storemonitoringid," +
            " sm.storeID " +
            " ,sg.groupcustomerid, ec.timeframe,ec.action " +
            " ) a " +
            " group by dateadd(minute,(datediff(minute,0,timeframe)/15)*15,0)," +
            " monitoringForDate," +
            " storeid,timeframe,action,groupCustomerID ,storenumber " +
            " ) x" +
            " group by StoreNumber,monitoringfordate,[15min],action" +
            " ) z group by StoreNumber,ActivityDate,ActivityTime order by 1,2,3" +
            " select * into #trafficdata from #trafficdata1 where 1=2" +
            " insert into #trafficdata select * from (" +
            " select StoreNumber,ActivityDate,ActivityTime,TrafficIn,TrafficOut,TrafficGroupIn,TrafficGroupOut,Filters" +
            " from (" +
            " select * from #trafficdata1 " +
            " union all" +
            " select * from #trafficdata2) a group by StoreNumber,ActivityDate,ActivityTime,TrafficIn,TrafficOut,TrafficGroupIn,TrafficGroupOut,Filters ) a" +
            " select  smd.* into #deleted from local235.cmsdb.dbo.CMS_DeleteMonitoring_Live smd " +
            " inner join #stores st on smd.storeID = st.storeid" +
            " where cast(smd.createdon as date) = dateadd(d,-1,CONVERT(VARCHAR(10), getdate(),110)) and companyid = 93" +
            " select  smd.* into #reprocessed from #deleted smd" +
            " inner join #storemonitoring sm on smd.storeid = sm.storeid and cast(smd.monitoringfordate as date) = cast(sm.monitoringfordate as date)" +
            " where smd.createdon < sm.processforconvdate and sm.processForConvDate < dateadd(d,-1,(DATEADD(HOUR,07,CONVERT(VARCHAR(10), getdate(),110))))" +
            " delete d from #deleted d inner join #reprocessed rp on d.storeid = rp.storeid and cast(d.monitoringfordate as date) = cast(rp.monitoringfordate as date)" +
            " insert into @monitoringlist select row_number() over (order by storenumber) rn, * from (" +
            " select storemonitoringid,monitoringfordate,StoreNumber,min(businessstarttime) starttime,max(businessendtime) endtime from (" +
            " select  cast(smd.storemonitoringid as varchar) as storemonitoringid,st.abbreviation as StoreNumber,smd.monitoringfordate,mont.processForConvDate,mont.businessStartTime,mont.businessEndTime from #deleted smd" +
            " inner join #stores st on smd.storeID = st.storeid" +
            " inner join #storemonitoring mont on smd.storeid = mont.storeID and smd.monitoringfordate = mont.monitoringForDate " +
            " union all" +
            " select sm.storemonitoringid,st.Abbreviation as StoreNumber,sm.monitoringfordate,sm.processforconvdate,sm.businessstarttime,sm.businessendtime from #storemonitoring sm " +
            " inner join #stores st on sm.storeID = st.storeid" +
            " where cast(sm.processForConvDate as datetime) between @from and @to " +
            " and cast(sm.monitoringfordate as date) > '2019-09-30' and cast(sm.monitoringfordate as date) <= cast(dateadd(day,-2,cast(@to as date)) as date)" +
            " union all" +
            " select sm.storemonitoringid,st.Abbreviation as StoreNumber,sm.monitoringfordate,sm.processforconvdate,sm.businessstarttime,sm.businessendtime from #storemonitoring sm " +
            " inner join #stores st on sm.storeID = st.storeid" +
            " where cast(sm.processForConvDate as datetime) between dateadd(day,-1,@from) and dateadd(day,-1,@to)" +
            " and cast(sm.monitoringfordate as date) = dateadd(day,-2,cast(@to as date))" +
            " ) b group by storemonitoringid,monitoringfordate,StoreNumber" +
            " ) a" +
            " while (@Xloop <= (select count(*) from @monitoringlist))" +
            " begin" +
            " select @storenumber=storenumber, @activitydate=monitoringfordate, @xfrom=starttime,@xto=endtime from @monitoringlist where rn=@Xloop" +
            " ;with CTE AS" +
            " (" +
            " SELECT  @storenumber as StoreNumber, cast(@activitydate as smalldatetime) as ActivityDate,CAST( (dateadd(minute,(datediff(minute,@xfrom,'00:15')  % 15), @xfrom)) AS Time) AS ActivityTime,0 as TrafficIn,0 as TrafficOut,0 as TrafficGroupIn,0 as TrafficGroupOut,0 as Filters" +
            " UNION ALL" +
            " SELECT @storenumber as StoreNumber,cast(@activitydate as smalldatetime) as ActivityDate,dateadd(Minute, 15, ActivityTime),0 as TrafficIn,0 as TrafficOut,0 as TrafficGroupIn,0 as TrafficGroupOut,0 as Filters" +
            " FROM CTE" +
            " where ActivityTime between cast((dateadd(minute,(datediff(minute,@xfrom,'00:15')  % 15), @xfrom)) as time) and cast((dateadd(minute,(datediff(minute,@xto,'00:15')  % 15)-15, @xto)) as time)" +
            " )" +
            " insert into #timeframetable select * from CTE" +
            " set @Xloop=@Xloop+1" +
            " end" +
            " select StoreNumber,ActivityDate,ActivityTime,sum(TrafficIn)TrafficIn,sum(TrafficOut)TrafficOut,sum(TrafficGroupIn)TrafficGroupIn,sum(TrafficGroupOut)TrafficGroupOut/*,sum(Filters) Filters*/ from (" +
            " select storenumber,cast(activitydate as smalldatetime)activitydate,activitytime,trafficin,trafficout,trafficgroupin,trafficgroupout,filters from #timeframetable" +
            " union all" +
            " select * from #trafficdata) a group by StoreNumber,ActivityDate,ActivityTime" +
            " --select sum(TrafficIn),sum(TrafficOut),sum(TrafficGroupIn),sum(TrafficGroupOut),sum(Filters) from #trafficdata " +
            " drop table #trafficdata" +
            " drop table #trafficdata1" +
            " drop table #trafficdata2" +
            " drop table #timeframetable" +
            " drop table #stores" +
            " drop table #deleted" +
            " drop table #reprocessed" +
            " drop table #storemonitoring";
        }

        public static string getQueryFile(string eDate)
        {
            return query = "declare @sdate date = '" + eDate + "'" +
                            " SELECT StoreNumber, cast(ActivityDate as date)ActivityDate, cast(ActivityTime as time)ActivityTime, TrafficIn, TrafficOut, TrafficGroupIn, TrafficGroupOut" +
                            " into #tcf1 FROM trafficbyStoreByMinutes_Flatten tff" +
                            " join StoreMonitoring sm on(tff.StoreID = sm.storeID)" +
                            " where cast(syncdate as date) = @sdate  and sm.processForConvDate is not Null and sm.monitoringStatusID = 220" +
                            " and tff.ActivityDate <= dateadd(day, -2, @sdate)" +
                            " group by StoreNumber, ActivityDate, ActivityTime, TrafficIn, TrafficOut, TrafficGroupIn, TrafficGroupOut" +
                            " order by storeNumber asc, ActivityDate asc, ActivityTime asc" +

                            " select StoreNumber, ActivityDate, ActivityTime, TrafficIn, TrafficOut, TrafficGroupIn, TrafficGroupOut, SUM(trafficIn + trafficOut + trafficGroupIn + trafficGroupOut) as Filtersss into #tcf2 from #tcf1" +
                            " group by StoreNumber, ActivityDate, ActivityTime, TrafficIn, TrafficOut, TrafficGroupIn, TrafficGroupOut" +
                            " order by storeNumber asc, ActivityDate asc, ActivityTime asc" +

                            " select * from #tcf2" +
                            " order by storeNumber asc, ActivityDate asc, ActivityTime asc" +
                            " --select COUNT(TrafficIn) as[Rows], SUM(TrafficIn) as[TrafficIn], SUM(TrafficOut) as[TrafficOut], SUM(TrafficGroupIn) as[TrafficGroupIn], SUM(TrafficGroupOut) as[TrafficGroupOut], SUM(Filters) as[Filters] from #tcf2" +

                            " drop table #tcf1" +
                            " drop table #tcf2";
                }

    }
}