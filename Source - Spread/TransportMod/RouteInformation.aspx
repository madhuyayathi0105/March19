<%@ Page Title="" Language="C#" MasterPageFile="~/TransportMod/TransportSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="RouteInformation.aspx.cs" Inherits="Default6" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function FpSpreadstage_ActiveCellChanged(event) {

            var spreadnew = document.getElementById('<%=FpSpreadstage.ClientID%>');
            var mainspred = document.getElementById('<%=sprdMainstage.ClientID %>');
            var activeRow = spreadnew.GetActiveRow();
            var activeCol = spreadnew.GetActiveCol();
            var rowcnt = spreadnew.GetTotalRowCount();
            var value = spreadnew.GetValue(activeRow, activeCol);
            var arrtime = spreadnew.GetValue(activeRow, 2);
            //alert('activeRow :' + activeRow);
            var previousactiverow = activeRow - 1;
            //alert('previousactiverow :' + previousactiverow);
            //alert('arrtime :' + arrtime);
            var Dept = spreadnew.GetValue(activeRow, 3);
            //alert('Dept :' + Dept);
            if (previousactiverow != "-1") {
                var Deptprevious = spreadnew.GetValue(previousactiverow, 3);
            }
            var deptime = mainspred.GetValue(activeRow, 3);
            var chk_splt_len = Dept.split(".");
            var lengthdept = chk_splt_len[0].length;
            var chk_spl_len_arr = arrtime.split(".");
            //alert('chk_spl_len_arr :' + chk_spl_len_arr);
            var leng_arr = chk_spl_len_arr[0].length;
            //alert('lengthdept :' + lengthdept);
            if (lengthdept < 3 || leng_arr < 3) {
                if (chk_splt_len[0] < 12 || chk_spl_len_arr[0] < 12) {

                    if (arrtime != "" && Dept != "" && arrtime != "-" && Dept != "Halt" && arrtime != "Halt") {
                        var arrtimevalue = arrtime;
                        var arraplivalue = arrtimevalue.split(".");
                        var deptsplitvalue = Dept.split(".");
                        var firstsplitarrtime = parseFloat(deptsplitvalue[0]) - parseFloat(arraplivalue[0]);
                        //alert('firstsplitarrtime :' + firstsplitarrtime);
                        var secondsplitdeptime = parseFloat(deptsplitvalue[1]) - parseFloat(arraplivalue[1]);
                        //alert('secondsplitdeptime :' + secondsplitdeptime);

                        var finalwaitingtime = (firstsplitarrtime * 60) + secondsplitdeptime;
                        //alert('finalwaitingtime :' + finalwaitingtime);
                        var lengthwaitsplit = finalwaitingtime.toString().length;
                        //alert('lengthwaitsplit :' + lengthwaitsplit);
                        if (lengthwaitsplit == 1) {
                            var lengthwaitingsplit1 = "0.0" + finalwaitingtime;
                            spreadnew.SetValue(activeRow, 4, lengthwaitingsplit1, true);
                        }
                        else {
                            var lengthwaitingsplit2 = "0." + finalwaitingtime;
                            spreadnew.SetValue(activeRow, 4, lengthwaitingsplit2, true);
                        }


                        //WAITING TIME SPLIT

                    }
                    else {
                        spreadnew.SetValue(activeRow, 4, "0.00", true);
                    }

                    //Alert------------------------------
                    if (arrtime != "" && arrtime != "-") {
                        //alert('if :' + arrtime);

                        var spl_derttime = Deptprevious.split(".");

                        var final_depttime = spl_derttime[0];
                        //alert('final_depttime :' + final_depttime);

                        if (arrtime != "") {

                            var arrtime_spl = arrtime.split(".");

                            var final_arr = arrtime_spl[0];
                            //alert('final_arr :' + final_arr);
                        }
                        if (Dept != "") {
                            var spli_dept = Dept.split(".");
                            var final_dep = spli_dept[0];
                            //alert('final_dep :' + final_dep);
                        }

                        if (final_arr < final_depttime) {
                            //alert('label : ' + arrtime);
                            var errId = 'Enter Time Can not be less than to Pervious Time';
                            alert('errId : ' + errId);
                            //                    document.getElementById("Label3").visibility = 'visible';
                            //                    document.getElementById("Label3").innerHTML = errId;
                        }
                        if (final_dep < final_depttime) {
                            var errId = 'Enter Time Can not be less than to Pervious Time';
                            alert('errId : ' + errId);
                        }


                    }







                    if (deptime != "") {
                        var activeRow = mainspred.GetActiveRow();
                        var activeCol = mainspred.GetActiveCol();
                        var rowcnt = spreadnew.GetTotalRowCount();
                        var rowcount = mainspred.GetTotalRowCount();
                        var value = mainspred.GetValue(activeRow, activeCol);
                        var k = 0;
                        for (var i = 1; i < rowcnt; i++) {
                            //alert('valuenew :' + i);
                            k = k + 1;
                            //alert('kValue  :' + k);
                            var arrtimef2 = spreadnew.GetValue((rowcnt - i), 2);
                            //alert('valuenew :' + arrtimef2);
                            var waitingtime = spreadnew.GetValue((rowcnt - i) - 1, 4);
                            var depttime2 = spreadnew.GetValue((rowcnt - i) - 1, 3);
                            //alert('valuenew :' + depttime2);
                            var deptime = mainspred.GetValue(activeRow, 3);
                            //alert('valuenew :' + deptime);

                            var arraplivalue1 = arrtimef2.split(".");
                            var deptsplitvalue1 = depttime2.split(".");
                            var firstsplitarrtime1 = parseFloat(arraplivalue1[0]) - parseFloat(deptsplitvalue1[0]);
                            //alert('firstsplitarrtime1 :' + firstsplitarrtime1);
                            var secondsplitdeptime1 = parseFloat(arraplivalue1[1]) - parseFloat(deptsplitvalue1[1]);
                            //alert('secondsplitdeptime1 :' + secondsplitdeptime1);
                            var finalwaitingtime1 = (firstsplitarrtime1 * 60) + secondsplitdeptime1;
                            //alert('finalwaitingtime1 :' + finalwaitingtime1);
                            var lengthwaitsplit1 = finalwaitingtime1.toString().length;
                            if (lengthwaitsplit1 == 1) {
                                //alert('lengthwaitsplit1 :' + lengthwaitsplit1);
                                var lengthwaitingsplitfp2 = "0.0" + finalwaitingtime1;
                            }
                            else {
                                var lengthwaitingsplitfp2 = "0." + finalwaitingtime1;
                            }

                            // spread.SetValue(activeRow, 4, lengthwaitingsplit2, true);


                            if (i == 1) {

                                var firstrowfp2 = parseFloat(deptime) + parseFloat(lengthwaitingsplitfp2);
                                //alert('firstrowfp2 :' + firstrowfp2);
                                var firstrowfp2final = firstrowfp2.toFixed(2);
                                var addzerofirstrowfp2final = "0" + firstrowfp2final;
                                //alert('firstrowfp2final :' + firstrowfp2final);

                                var firstrowsplit = firstrowfp2final.split(".");

                                //alert('firstrowsplit :' + firstrowsplit);
                                if (firstrowsplit[1] > 59) {
                                    var firstrowsplit1 = parseFloat(firstrowsplit[0], 10) + 1;
                                    //alert('firstrowsplit1 :' + firstrowsplit1);
                                    var firstrowsplit2 = firstrowsplit[1] - 60;
                                    //alert('firstrowsplit2 :' + firstrowsplit2);

                                    var firstrowsplitFinal = firstrowsplit1 + "." + firstrowsplit2;
                                    var addzerofirstrowsplitFinal = "0" + firstrowsplitFinal;
                                    //alert('firstrowsplitFinal :' + firstrowsplitFinal);

                                    var firstrowlengthsplitarr = firstrowsplitFinal.split(".");
                                    var lengtharrsplit = firstrowlengthsplitarr[1].length;
                                    if (lengtharrsplit < 2) {
                                        var lengtharrsplit1 = "0" + firstrowlengthsplitarr[1];
                                        var lengtharrsplit2 = firstrowlengthsplitarr[0];
                                        var totallengthfirstrowvalue = lengtharrsplit2 + "." + lengtharrsplit1;
                                        var addzerototallengthfirstrowvalue = "0" + totallengthfirstrowvalue;
                                        mainspred.SetValue(k, 2, addzerototallengthfirstrowvalue, true);

                                    }
                                    else {
                                        mainspred.SetValue(k, 2, addzerofirstrowsplitFinal, true);
                                    }

                                }
                                else {
                                    mainspred.SetValue(k, 2, addzerofirstrowfp2final, true);
                                }

                                var Deptrowvalue = parseFloat(waitingtime) + parseFloat(firstrowfp2final);
                                var Deptrowvalue2 = Deptrowvalue.toFixed(2);
                                var addzeroDeptrowvalue2 = "0" + Deptrowvalue2;
                                //alert('secondrowfp2final :' + Deptrowvalue2);
                                //split value
                                var secondrowsplit = Deptrowvalue2.split(".");
                                //alert('secondrowsplit :' + secondrowsplit);

                                if (secondrowsplit[1] > 59) {
                                    var secondrowsplit1 = parseFloat(secondrowsplit[0], 10) + 1;
                                    //alert('secondrowsplit1 :' + secondrowsplit1);
                                    var secondrowsplit2 = secondrowsplit[1] - 60;
                                    //alert('secondrowsplit2 :' + secondrowsplit2);

                                    var secondrowsplitFinal = secondrowsplit1 + "." + secondrowsplit2;
                                    var addzerosecondrowsplitFinal = "0" + secondrowsplitFinal;
                                    //alert('secondrowsplitFinal :' + secondrowsplitFinal);

                                    var firstrowlengthsplitDept = secondrowsplitFinal.split(".");
                                    var lengthDeptfirstsplit = firstrowlengthsplitDept[1].length;
                                    if (lengthDeptfirstsplit < 2) {
                                        var lengthDeptfirstsplit1 = "0" + firstrowlengthsplitDept[1];
                                        var lengthDeptfirstsplit2 = firstrowlengthsplitDept[0];
                                        var totallengthfirstDeptrowvalue = lengthDeptfirstsplit2 + "." + lengthDeptfirstsplit1;
                                        var addzerototallengthfirstDeptrowvalue = "0" + totallengthfirstDeptrowvalue;
                                        mainspred.SetValue(k, 3, addzerototallengthfirstDeptrowvalue, true);
                                    }

                                    else {
                                        mainspred.SetValue(k, 3, addzerosecondrowsplitFinal, true);
                                    }

                                }
                                else {

                                    mainspred.SetValue(k, 3, addzeroDeptrowvalue2, true);
                                }
                                //waiting calculation for sample
                                var arrtimef2 = mainspred.GetValue(i, 2);
                                var depttimef2 = mainspred.GetValue(i, 3);
                                if (arrtimef2 != "" && depttimef2 != "" && arrtimef2 != "-" && depttimef2 != "Halt" && arrtimef2 != "Halt") {
                                    var arraplivaluemainsprd = arrtimef2.split(".");
                                    var deptsplitvaluemainspred = depttimef2.split(".");
                                    var firstsplitarrtimef2 = parseFloat(deptsplitvaluemainspred[0]) - parseFloat(arraplivaluemainsprd[0]);
                                    var secondsplitdeptimef2 = parseFloat(deptsplitvaluemainspred[1]) - parseFloat(arraplivaluemainsprd[1]);
                                    var finalwaitingtimef2 = (firstsplitarrtimef2 * 60) + secondsplitdeptimef2;
                                    //alert('finalwaitingtime :' + finalwaitingtime);
                                    var lengthwaitsplitf2 = finalwaitingtimef2.toString().length;
                                    if (lengthwaitsplitf2 == 1) {
                                        var lengthwaitingsplitf2 = "0.0" + finalwaitingtimef2;
                                        mainspred.SetValue(k, 4, lengthwaitingsplitf2, true);
                                    }
                                    else {
                                        var lengthwaitingsplitf2new = "0." + finalwaitingtimef2;
                                        mainspred.SetValue(k, 4, lengthwaitingsplitf2new, true);
                                    }

                                }
                                else {
                                    mainspred.SetValue(k, 4, "0.00", true);
                                }


                            }
                            if (i > 1) {
                                //Arr2 time calculation
                                //alert('i2 :' + i);
                                var DeptAllFp2 = mainspred.GetValue((i - 1), 3);
                                //alert('DeptAllFp2 :' + DeptAllFp2);
                                var nextrowarrtime = parseFloat(DeptAllFp2) + parseFloat(lengthwaitingsplitfp2);
                                // alert('firstarrrowfp2 :' + nextrowarrtime);
                                var firstarrrowfp2final = nextrowarrtime.toFixed(2);
                                var addzerofirstarrrowfp2final = "0" + firstarrrowfp2final;
                                //splitvalue

                                var secondarrrowsplit = firstarrrowfp2final.split(".");
                                //alert('secondarrrowsplit :' + secondarrrowsplit);
                                if (secondarrrowsplit[1] > 59) {
                                    var secondarrrowsplit1 = parseFloat(secondarrrowsplit[0], 10) + 1;
                                    // alert('secondarrrowsplit1 :' + secondarrrowsplit1);
                                    var secondarrrowsplit2 = secondarrrowsplit[1] - 60;
                                    //alert('secondarrrowsplit2 :' + secondarrrowsplit2);

                                    var secondarrrowsplitFinal = secondarrrowsplit1 + "." + secondarrrowsplit2;
                                    var addzerosecondarrrowsplitFinal = "0" + secondarrrowsplitFinal;
                                    //alert('secondrowsplitFinal :' + secondarrrowsplitFinal);
                                    var secondarrlengthsplit = secondarrrowsplitFinal.split(".");
                                    var lengtharr = secondarrlengthsplit[1].length;
                                    if (lengtharr < 2) {
                                        var lengtharr1 = "0" + secondarrlengthsplit[1];
                                        var lengtharr2 = secondarrlengthsplit[0];
                                        var totallengtharrvalue = lengtharr2 + "." + lengtharr1;
                                        var addzerototallengtharrvalue = "0" + totallengtharrvalue;
                                        mainspred.SetValue(k, 2, addzerototallengtharrvalue, true);
                                    }
                                    else {
                                        mainspred.SetValue(k, 2, addzerosecondarrrowsplitFinal, true);
                                    }


                                }

                                else {

                                    mainspred.SetValue(k, 2, addzerofirstarrrowfp2final, true);
                                }


                                //Dept2 Time Calculation
                                var nextrowdepttime = parseFloat(firstarrrowfp2final) + parseFloat(waitingtime);

                                //alert('nextrowdepttime :' + nextrowdepttime);

                                var firstdeptrowfp2final = nextrowdepttime.toFixed(2);
                                var addzerofirstdeptrowfp2final = "0" + firstdeptrowfp2final;
                                //alert('firstdeptrowfp2final :' + firstdeptrowfp2final);

                                //split value
                                var secondDeptrowsplit = firstdeptrowfp2final.split(".");
                                //alert('secondDeptrowsplit :' + secondDeptrowsplit);
                                if (secondDeptrowsplit[1] > 59) {


                                    var secondDeptrowsplit1 = parseFloat(secondDeptrowsplit[0], 10) + 1;
                                    //alert('secondDeptrowsplit1 :' + secondDeptrowsplit1);
                                    var secondDeptrowsplit2 = secondDeptrowsplit[1] - 60;
                                    //alert('secondDeptrowsplit2 :' + secondDeptrowsplit2);

                                    var secondDeptrowsplitFinal = secondDeptrowsplit1 + "." + secondDeptrowsplit2;
                                    var addzerosecondDeptrowsplitFinal = "0" + secondDeptrowsplitFinal;
                                    //alert('secondDeptrowsplitFinal :' + secondDeptrowsplitFinal);

                                    var secondDeptrowsplitlength = secondDeptrowsplitFinal.split(".");
                                    var lengthdept = secondDeptrowsplitlength[1].length;
                                    if (lengthdept < 2) {
                                        var lengthdept1 = "0" + secondDeptrowsplitlength[1];
                                        var lengthdept2 = secondDeptrowsplitlength[0];
                                        var totallengthdeptvalue = lengthdept2 + "." + lengthdept1;
                                        var addzerototallengthdeptvalue = "0" + totallengthdeptvalue;
                                        mainspred.SetValue(k, 3, addzerototallengthdeptvalue, true);
                                    }
                                    else {
                                        mainspred.SetValue(k, 3, addzerosecondDeptrowsplitFinal, true);
                                    }

                                }
                                else {
                                    mainspred.SetValue(k, 3, addzerofirstdeptrowfp2final, true);
                                }


                                //waiting time calculation spread2

                                var arrtimef3 = mainspred.GetValue(i, 2);
                                var depttimef3 = mainspred.GetValue(i, 3);
                                if (arrtimef3 != "" && depttimef3 != "" && arrtimef3 != "-" && depttimef3 != "Halt" && arrtimef3 != "Halt") {
                                    var arraplivaluemainsprd1 = arrtimef3.split(".");
                                    var deptsplitvaluemainspred1 = depttimef3.split(".");
                                    var firstsplitarrtimef3 = parseFloat(deptsplitvaluemainspred1[0]) - parseFloat(arraplivaluemainsprd1[0]);
                                    var secondsplitdeptimef3 = parseFloat(deptsplitvaluemainspred1[1]) - parseFloat(arraplivaluemainsprd1[1]);
                                    var finalwaitingtimef3 = (firstsplitarrtimef3 * 60) + secondsplitdeptimef3;
                                    //alert('finalwaitingtime :' + finalwaitingtime);
                                    var lengthwaitsplitf3 = finalwaitingtimef3.toString().length;
                                    if (lengthwaitsplitf3 == 1) {
                                        var lengthwaitingsplitf3 = "0.0" + finalwaitingtimef3;
                                        mainspred.SetValue(k, 4, lengthwaitingsplitf3, true);
                                    }
                                    else {
                                        var lengthwaitingsplitf3new = "0." + finalwaitingtimef3;
                                        mainspred.SetValue(k, 4, lengthwaitingsplitf3new, true);
                                    }

                                }
                                else {
                                    mainspred.SetValue(k, 4, "0.00", true);
                                }



                            }

                        }


                    }
                    else {

                        if (arrtime != "" && Dept != "" && arrtime != "-" && Dept != "Halt" && arrtime != "Halt") {
                            var arrtimevalue = arrtime;
                            var arraplivalue = arrtimevalue.split(".");
                            var deptsplitvalue = Dept.split(".");
                            var firstsplitarrtime = parseFloat(deptsplitvalue[0]) - parseFloat(arraplivalue[0]);
                            //alert('firstsplitarrtime :' + firstsplitarrtime);
                            var secondsplitdeptime = parseFloat(deptsplitvalue[1]) - parseFloat(arraplivalue[1]);
                            //alert('secondsplitdeptime :' + secondsplitdeptime);

                            var finalwaitingtime = (firstsplitarrtime * 60) + secondsplitdeptime;
                            //alert('finalwaitingtime :' + finalwaitingtime);
                            var lengthwaitsplit = finalwaitingtime.toString().length;
                            //alert('lengthwaitsplit :' + lengthwaitsplit);
                            if (lengthwaitsplit == 1) {
                                var lengthwaitingsplit1 = "0.0" + finalwaitingtime;
                                spreadnew.SetValue(activeRow, 4, lengthwaitingsplit1, true);
                            }
                            else {
                                var lengthwaitingsplit2 = "0." + finalwaitingtime;
                                spreadnew.SetValue(activeRow, 4, lengthwaitingsplit2, true);
                            }


                            //WAITING TIME SPLIT

                        }
                        else {
                            spreadnew.SetValue(activeRow, 4, "0.00", true);
                        }


                    }
                }
                else {
                    var errId = 'Please Enter The 12hours Format';
                    alert('errId : ' + errId);
                }
            }
            else {
                var errId = 'Enter valid Time Format';
                alert('errId : ' + errId);
            }


        }

        function FpMainstages_ActiveCellChanged(event) {
            var spreadnew1 = document.getElementById('<%=FpSpreadstage.ClientID%>');
            var mainspred1 = document.getElementById('<%=sprdMainstage.ClientID %>');
            var activeRow = mainspred1.GetActiveRow();
            var activeCol = mainspred1.GetActiveCol();
            var rowcnt = spreadnew1.GetTotalRowCount();
            var value = mainspred1.GetValue(activeRow, activeCol);
            var k = 0;
            for (var i = 1; i < rowcnt; i++) {
                //alert('valuenew :' + i);
                k = k + 1;
                //alert('kValue  :' + k);
                var arrtimef2 = spreadnew1.GetValue((rowcnt - i), 2);
                var waitingtime = spreadnew1.GetValue((rowcnt - i) - 1, 4);
                var depttime2 = spreadnew1.GetValue((rowcnt - i) - 1, 3);
                //alert('valuenew :' + depttime2);
                var deptime = mainspred1.GetValue(activeRow, 3);
                //alert('valuenew :' + deptime);
                var arraplivalue1 = arrtimef2.split(".");
                var deptsplitvalue1 = depttime2.split(".");
                var firstsplitarrtime1 = parseFloat(arraplivalue1[0]) - parseFloat(deptsplitvalue1[0]);
                //alert('firstsplitarrtime1 :' + firstsplitarrtime1);
                var secondsplitdeptime1 = parseFloat(arraplivalue1[1]) - parseFloat(deptsplitvalue1[1]);
                if (secondsplitdeptime1 < 0) {
                    secondsplitdeptime1 = secondsplitdeptime1 + 60;
                    firstsplitarrtime1 = firstsplitarrtime1 - 1;
                }
                //alert('secondsplitdeptime1 :' + secondsplitdeptime1);
                var finalwaitingtime1 = (firstsplitarrtime1 * 60) + secondsplitdeptime1;
                //alert('finalwaitingtime1 :' + finalwaitingtime1);
                var lengthwaitsplit1 = finalwaitingtime1.toString().length;
                if (lengthwaitsplit1 == 1) {
                    //alert('lengthwaitsplit1 :' + lengthwaitsplit1);
                    var lengthwaitingsplitfp2 = "0.0" + finalwaitingtime1;
                }
                else {
                    var lengthwaitingsplitfp2 = "0." + finalwaitingtime1;
                }

                // spread.SetValue(activeRow, 4, lengthwaitingsplit2, true);


                if (i == 1) {
                    var deptimesplit = deptime.split("."); //rajasekar
                    var deptimeadd = parseFloat(deptimesplit[0]) + firstsplitarrtime1;
                    var deptimeadd1 = parseFloat(deptimesplit[1]) + secondsplitdeptime1;
                    if (deptimeadd1 < 10) {
                        var firstrowfp2 = deptimeadd + ".0" + deptimeadd1;
                    } else {
                        var firstrowfp2 = parseFloat(deptimeadd) + "." + parseFloat(deptimeadd1);
                     }
                    //var firstrowfp2 = parseFloat(deptime) + parseFloat(lengthwaitingsplitfp2);
                    //alert('firstrowfp2 :' + firstrowfp2);
                    var firstrowfp2final = firstrowfp2; //.toFixed(2);
                    if (deptimeadd < 10) {
                        var addzerofirstrowfp2final = "0" + firstrowfp2final;
                    }
                    else {
                        var addzerofirstrowfp2final = firstrowfp2final;
                    }
                    //alert('firstrowfp2final :' + firstrowfp2final);

                    var firstrowsplit = firstrowfp2final.split(".");

                    //alert('firstrowsplit :' + firstrowsplit);
                    if (firstrowsplit[1] > 59) {
                        var firstrowsplit1 = parseFloat(firstrowsplit[0], 10) + 1;
                        //alert('firstrowsplit1 :' + firstrowsplit1);
                        var firstrowsplit2 = firstrowsplit[1] - 60;
                        //alert('firstrowsplit2 :' + firstrowsplit2);

                        var firstrowsplitFinal = firstrowsplit1 + "." + firstrowsplit2;
                        var firstrowsplitFinal1 = firstrowsplitFinal.split(".");
                        if (firstrowsplitFinal1[0] < 10) {
                            var addzerofirstrowsplitFinal = "0" + firstrowsplitFinal;
                        } else {
                            var addzerofirstrowsplitFinal = firstrowsplitFinal;
                        }
                        //alert('firstrowsplitFinal :' + firstrowsplitFinal);

                        var firstrowlengthsplitarr = firstrowsplitFinal.split(".");
                        var lengtharrsplit = firstrowlengthsplitarr[1].length;
                        if (lengtharrsplit < 2) {
                            var lengtharrsplit1 = "0" + firstrowlengthsplitarr[1];
                            var lengtharrsplit2 = firstrowlengthsplitarr[0];
                            var totallengthfirstrowvalue = lengtharrsplit2 + "." + lengtharrsplit1;
                            var totallengthfirstrowvalue1 = totallengthfirstrowvalue.split(".");
                            if (totallengthfirstrowvalue1[0] < 10) {
                                var addzerototallengthfirstrowvalue = "0" + totallengthfirstrowvalue;
                            } else {
                                var addzerototallengthfirstrowvalue = totallengthfirstrowvalue;
                            }
                            mainspred1.SetValue(k, 2, addzerototallengthfirstrowvalue, true);

                        }
                        else {
                            mainspred1.SetValue(k, 2, addzerofirstrowsplitFinal, true);
                        }

                    }
                    else {
                        mainspred1.SetValue(k, 2, addzerofirstrowfp2final, true);
                    }

                    var Deptrowvalue = parseFloat(waitingtime) + parseFloat(firstrowfp2final);
                    var Deptrowvalue2 = Deptrowvalue.toFixed(2);
                    var Deptrowvalue1 = Deptrowvalue2.split(".");
                    if (Deptrowvalue1[0] < 10) {
                        var addzeroDeptrowvalue2 = "0" + Deptrowvalue2;
                    } else {
                        var addzeroDeptrowvalue2 = Deptrowvalue2;
                    }
                    //alert('secondrowfp2final :' + Deptrowvalue2);
                    //split value
                    var secondrowsplit = Deptrowvalue2.split(".");
                    //alert('secondrowsplit :' + secondrowsplit);

                    if (secondrowsplit[1] > 59) {
                        var secondrowsplit1 = parseFloat(secondrowsplit[0], 10) + 1;
                        //alert('secondrowsplit1 :' + secondrowsplit1);
                        var secondrowsplit2 = secondrowsplit[1] - 60;
                        //alert('secondrowsplit2 :' + secondrowsplit2);

                        var secondrowsplitFinal = secondrowsplit1 + "." + secondrowsplit2;
                        var secondrowsplitFinal1 = secondrowsplitFinal.split(".");
                        if (secondrowsplitFinal1[0] < 10) {
                            var addzerosecondrowsplitFinal = "0" + secondrowsplitFinal;
                        } else {
                            var addzerosecondrowsplitFinal = secondrowsplitFinal;
                        }
                        
                        //alert('secondrowsplitFinal :' + secondrowsplitFinal);

                        var firstrowlengthsplitDept = secondrowsplitFinal.split(".");
                        var lengthDeptfirstsplit = firstrowlengthsplitDept[1].length;
                        if (lengthDeptfirstsplit < 2) {
                            var lengthDeptfirstsplit1 = "0" + firstrowlengthsplitDept[1];
                            var lengthDeptfirstsplit2 = firstrowlengthsplitDept[0];
                            var totallengthfirstDeptrowvalue = lengthDeptfirstsplit2 + "." + lengthDeptfirstsplit1;
                            var totallengthfirstDeptrowvalue1 = totallengthfirstDeptrowvalue.split(".");
                            if (totallengthfirstDeptrowvalue1[0] < 10) {
                                var addzerototallengthfirstDeptrowvalue = "0" + totallengthfirstDeptrowvalue;
                            } else {
                                var addzerototallengthfirstDeptrowvalue = totallengthfirstDeptrowvalue;
                            }
                            mainspred1.SetValue(k, 3, addzerototallengthfirstDeptrowvalue, true);

                        }

                        else {
                            mainspred1.SetValue(k, 3, addzerosecondrowsplitFinal, true);
                        }

                    }
                    else {

                        mainspred1.SetValue(k, 3, addzeroDeptrowvalue2, true);
                    }

                }
                if (i > 1) {
                    //Arr2 time calculation
                    //alert('i2 :' + i);
                    var DeptAllFp2 = mainspred1.GetValue((i - 1), 3);
                    var DeptAllFp21 = DeptAllFp2.split(".");
                    var lengthwaitingsplitfp21 = lengthwaitingsplitfp2.split(".");
                    var limit = parseFloat(DeptAllFp21[1]) + parseFloat(lengthwaitingsplitfp21[1]);
                    if (limit < 100) {
                        //alert('DeptAllFp2 :' + DeptAllFp2);
                        var nextrowarrtime = parseFloat(DeptAllFp2) + parseFloat(lengthwaitingsplitfp2);
                        var firstarrrowfp2final = nextrowarrtime.toFixed(2);
                    }
                    else {
                        var minutes = limit - 60;

                        var hour = parseFloat(DeptAllFp21[0]) + parseFloat(1);
                        
                        var nextrowarrtime =parseFloat(hour) +"."+parseFloat(minutes);
                        var firstarrrowfp2final = nextrowarrtime;
                    }
                    // alert('firstarrrowfp2 :' + nextrowarrtime);
                    
                    
                    var firstarrrowfp2final1 = firstarrrowfp2final.split(".");
                    
                    if (firstarrrowfp2final1[0] < 10) {
                        var addzerofirstarrrowfp2final = "0" + firstarrrowfp2final;
                    } else {
                        var addzerofirstarrrowfp2final = firstarrrowfp2final;
                    }
                    
                    
                    //splitvalue

                    var secondarrrowsplit = firstarrrowfp2final.split(".");
                    //alert('secondarrrowsplit :' + secondarrrowsplit);
                    if (secondarrrowsplit[1] > 59) {
                        var secondarrrowsplit1 = parseFloat(secondarrrowsplit[0], 10) + 1;
                        // alert('secondarrrowsplit1 :' + secondarrrowsplit1);
                        var secondarrrowsplit2 = secondarrrowsplit[1] - 60;
                        //alert('secondarrrowsplit2 :' + secondarrrowsplit2);

                        var secondarrrowsplitFinal = secondarrrowsplit1 + "." + secondarrrowsplit2;
                        var secondarrrowsplitFinal1 = secondarrrowsplitFinal.split(".");
                        if (secondarrrowsplitFinal1[0] < 10) {
                            var addzerosecondarrrowsplitFinal = "0" + secondarrrowsplitFinal;
                        } else {
                            var addzerosecondarrrowsplitFinal = secondarrrowsplitFinal;
                        }
                        
                        //alert('secondrowsplitFinal :' + secondarrrowsplitFinal);
                        var secondarrlengthsplit = secondarrrowsplitFinal.split(".");
                        var lengtharr = secondarrlengthsplit[1].length;
                        if (lengtharr < 2) {
                            var lengtharr1 = "0" + secondarrlengthsplit[1];
                            var lengtharr2 = secondarrlengthsplit[0];
                            var totallengtharrvalue = lengtharr2 + "." + lengtharr1;
                            var totallengtharrvalue1 = totallengtharrvalue.split(".");
                            if (totallengtharrvalue1[0] < 10) {
                                var addzerototallengtharrvalue = "0" + totallengtharrvalue;
                            } else {
                                var addzerototallengtharrvalue = totallengtharrvalue;
                            }

                            mainspred1.SetValue(k, 2, addzerototallengtharrvalue, true);
                        }
                        else {
                            mainspred1.SetValue(k, 2, addzerosecondarrrowsplitFinal, true);
                        }
                        firstarrrowfp2final = addzerosecondarrrowsplitFinal;

                    }

                    else {

                        mainspred1.SetValue(k, 2, addzerofirstarrrowfp2final, true);
                    }


                    //Dept2 Time Calculation
                    var firstarrrowfp2final1 = firstarrrowfp2final.split(".");
                    if (firstarrrowfp2final1[1] < 10) {
                        firstarrrowfp2final = firstarrrowfp2final1[0] + ".0" + firstarrrowfp2final1[1];
                    } else {
                        firstarrrowfp2final = firstarrrowfp2final1[0] + "." + firstarrrowfp2final1[1];
                    }
                    var nextrowdepttime = parseFloat(firstarrrowfp2final) + parseFloat(waitingtime);
                    

                    //alert('nextrowdepttime :' + nextrowdepttime);

                    var firstdeptrowfp2final = nextrowdepttime.toFixed(2);
                    var firstdeptrowfp2final1 = firstdeptrowfp2final.split(".");
                    if (firstdeptrowfp2final1[0] < 10) {
                        var addzerofirstdeptrowfp2final = "0" + firstdeptrowfp2final;
                    } else {
                        var addzerofirstdeptrowfp2final = firstdeptrowfp2final;
                    }
                    
                    //alert('firstdeptrowfp2final :' + firstdeptrowfp2final);

                    //split value
                    var secondDeptrowsplit = firstdeptrowfp2final.split(".");
                    //alert('secondDeptrowsplit :' + secondDeptrowsplit);
                    if (secondDeptrowsplit[1] > 59) {


                        var secondDeptrowsplit1 = parseFloat(secondDeptrowsplit[0], 10) + 1;
                        //alert('secondDeptrowsplit1 :' + secondDeptrowsplit1);
                        var secondDeptrowsplit2 = secondDeptrowsplit[1] - 60;
                        //alert('secondDeptrowsplit2 :' + secondDeptrowsplit2);

                        var secondDeptrowsplitFinal = secondDeptrowsplit1 + "." + secondDeptrowsplit2;
                        var secondDeptrowsplitFinal1 = secondDeptrowsplitFinal.split(".");
                        if (secondDeptrowsplitFinal1[0] < 10) {
                            var addzerosecondDeptrowsplitFinal = "0" + secondDeptrowsplitFinal;
                        } else {
                            var addzerosecondDeptrowsplitFinal = secondDeptrowsplitFinal;
                        }
                        
                        //alert('secondDeptrowsplitFinal :' + secondDeptrowsplitFinal);

                        var secondDeptrowsplitlength = secondDeptrowsplitFinal.split(".");
                        var lengthdept = secondDeptrowsplitlength[1].length;
                        if (lengthdept < 2) {
                            var lengthdept1 = "0" + secondDeptrowsplitlength[1];
                            var lengthdept2 = secondDeptrowsplitlength[0];
                            var totallengthdeptvalue = lengthdept2 + "." + lengthdept1;
                            var totallengthdeptvalue1 = totallengthdeptvalue.split(".");
                            if (totallengthdeptvalue1[0] < 10) {
                                var addzerototallengthdeptvalue = "0" + totallengthdeptvalue;
                            } else {
                                var addzerototallengthdeptvalue = totallengthdeptvalue;
                            }

                            mainspred1.SetValue(k, 3, addzerototallengthdeptvalue, true);
                        }
                        else {
                            mainspred1.SetValue(k, 3, addzerosecondDeptrowsplitFinal, true);
                        }

                    }
                    else {
                        mainspred1.SetValue(k, 3, addzerofirstdeptrowfp2final, true);
                    }


                }

            }

        }




    </script>
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="width: 946px">
                <tr>
                    <td align="left">
                        <asp:Panel ID="pnl4" runat="server" BackImageUrl="~/image/Top Band-2.jpg" Height="20px"
                            Style="margin-left: 0px; top: 70px; left: -23px; width: 1018px; position: absolute;">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="Label31" runat="server" Text="Route Master" Font-Bold="true" Font-Names="MS Sans Serif"
                                Font-Size="Medium" ForeColor="White"></asp:Label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <%-- <asp:LinkButton ID="LinkButton1" runat="server" Font-Names="MS Sans Serif" Font-Size="Small"
                                ForeColor="White" Font-Bold="true" PostBackUrl="~/RouteInformation.aspx">Home</asp:LinkButton>
                            &nbsp; &nbsp;
                            <asp:LinkButton ID="LinkButton3" runat="server" Font-Names="MS Sans Serif" Font-Size="Small"
                                ForeColor="White" Font-Bold="true" PostBackUrl="~/Default_login.aspx">Back</asp:LinkButton>
                            &nbsp; &nbsp;
                            <asp:LinkButton ID="lb2" runat="server" OnClick="lb2_Click" Font-Bold="true" Font-Names="MS Sans Serif"
                                Font-Size="Small" ForeColor="White">Logout</asp:LinkButton>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <br />
            <asp:Accordion ID="Accordion1" CssClass="accordion" HeaderCssClass="accordionHeader"
                HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
                runat="server" Width="971px" Height="500px" BorderColor="White" Style="margin-right: 0px;
                margin-top: -3px; height: 500px; margin-left:15px;">
                <Panes>
                    <asp:AccordionPane ID="AccordionPane1" runat="server">
                        <Header>
                            View</Header>
                        <Content>
                            <asp:Label ID="lblerrordate" runat="server" ForeColor="Red" Text="" Font-Names="MS Sans Serif"
                                Font-Size="5pt" Visible="false"></asp:Label>
                            <asp:Panel ID="Panel2" runat="server" Style="border-style: solid; border-width: thin;
                                border-color: Black; background: White;">
                                <br />
                                <table class="tabl" style="width: 774px;">
                                    <tr>
                                        <td style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                                            background-color: lightblue; border-width: 1px;">
                                            <asp:Label ID="lblvehicletype" runat="server" Font-Bold="true" CssClass="font" Text="Search By Place"></asp:Label>
                                        </td>
                                        <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                                            border-width: 1px; border-right-style: solid;">
                                            <asp:DropDownList ID="ddlserachby" runat="server" Font-Bold="true" CssClass="font"
                                                Width="122px" AutoPostBack="True" OnSelectedIndexChanged="ddlserachby_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                                            background-color: lightblue; border-width: 1px;">
                                            <asp:Label ID="lblrouteid" runat="server" Font-Bold="true" CssClass="font" Text="Search By Route_ID"></asp:Label>
                                        </td>
                                        <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                                            border-width: 1px; border-right-style: solid;">
                                            <asp:DropDownList ID="ddlrouteID" runat="server" Font-Bold="true" CssClass="font"
                                                Width="122px" AutoPostBack="True" OnSelectedIndexChanged="ddlrouteID_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnMainGo" runat="server" Text="Go" Font-Bold="True" OnClick="btnMainGo_Click"
                                                Font-Names="Book Antiqua" Font-Size="Medium" />
                                        </td>
                                        <td>
                                            <asp:LinkButton Text="Set Route Priority" runat="server" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Font-Bold="True" PostBackUrl="Route Priority.aspx"></asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <table>
                                <tr>
                                    <td>
                                        <FarPoint:FpSpread ID="FpTransport" runat="server" Height="250px" Width="900px" OnCellClick="FpTransport_CellClick"
                                            OnPreRender="FpTransport_SelectedIndexChanged" ActiveSheetViewIndex="0" currentPageIndex="0"
                                            DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Spread /&gt;"
                                            EnableClientScript="False" CssClass="cursorptr" BorderColor="Black" BorderWidth="0.5">
                                            <CommandBar BackColor="Control" ButtonType="PushButton">
                                                <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
                                            </CommandBar>
                                            <Pager Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" />
                                            <HierBar Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" />
                                            <Sheets>
                                                <FarPoint:SheetView SheetName="Sheet1" EditTemplateColumnCount="2" GridLineColor="Black"
                                                    GroupBarText="Drag a column to group by that column." SelectionBackColor="#CE5D5A"
                                                    SelectionForeColor="White">
                                                </FarPoint:SheetView>
                                            </Sheets>
                                            <TitleInfo BackColor="#E7EFF7" Font-Size="X-Large" ForeColor="" HorizontalAlign="Center"
                                                VerticalAlign="NotSet" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                                Font-Strikeout="False" Font-Underline="False">
                                            </TitleInfo>
                                        </FarPoint:FpSpread>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                                        <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" />
                                    </td>
                                </tr>
                            </table>
                        </Content>
                    </asp:AccordionPane>
                    <asp:AccordionPane ID="AccordionPane2" runat="server">
                        <Header>
                            <asp:Label ID="lblrouteadd" runat="server" Text="Add"></asp:Label></Header>
                        <Content>
                            <asp:Label ID="Labelvalidation" runat="server" ForeColor="Red" Text="" Font-Names="MS Sans Serif"
                                Font-Bold="true" Font-Size="5pt" Visible="false" Style="position: absolute; top: 231px;"></asp:Label>
                            <div class="PopupHeaderrstud" id="Div12" style="text-align: center; font-family: MS Sans Serif;
                                background: white; font-size: Small; font-weight: bold">
                                <asp:Panel ID="Panel1" runat="server" Style="left: -147px; border-color: Gray; border-style: solid;
                                    width: 964px; height: 755px; margin-bottom: 0px; margin-right: 212px; margin-left: -6px;
                                    margin-top: -20px;">
                                    <table class="tablfont" style="border: thin solid Gray; left: 20px; width: 964px;
                                        height: 39px; right: 375px; margin-top: 20PX;">
                                        <tr>
                                            <td style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                                                background-color: lightblue; border-width: 1px;">
                                                <asp:Label ID="Label86" runat="server" Text="Vehicle ID:" CssClass="font"></asp:Label>
                                            </td>
                                            <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                                                border-width: 1px; border-right-style: solid;">
                                                <asp:DropDownList ID="ddlvehiclid" runat="server" Font-Bold="true" CssClass="font"
                                                    Width="103px" AutoPostBack="True" OnSelectedIndexChanged="ddlvehiclid_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                                                background-color: lightblue; border-width: 1px;">
                                                <asp:Label ID="Label1" runat="server" Text="Route ID:" CssClass="font"></asp:Label>
                                            </td>
                                            <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                                                border-width: 1px; border-right-style: solid;">
                                                <asp:TextBox ID="tbrouteid" runat="server" OnTextChanged="tbrouteid_TextChanged"
                                                    AutoPostBack="true" CssClass="font" Height="12px" Width="60px"></asp:TextBox>
                                            </td>
                                            <td style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                                                background-color: lightblue; border-width: 1px;">
                                                <asp:Label ID="lbldate" runat="server" Font-Bold="true" CssClass="font" Text="From Date">
                                                </asp:Label>
                                            </td>
                                            <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                                                border-width: 1px; border-right-style: solid;">
                                                <asp:TextBox ID="tbdate" runat="server" CssClass="font" Width="70px" Height="18px"
                                                    AutoPostBack="True"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="tbdate" Format="dd-MM-yyyy"
                                                    runat="server" Enabled="True">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                                                background-color: lightblue; border-width: 1px;">
                                                <asp:Label ID="lblstage" runat="server" Text="Total No.Of Stage:" CssClass="font"></asp:Label>
                                            </td>
                                            <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                                                border-width: 1px; border-right-style: solid;">
                                                <asp:TextBox ID="tbstages" runat="server" OnTextChanged="tbstages_TextChanged" MaxLength="3"
                                                    CssClass="font" Height="12px" Width="25px"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" TargetControlID="tbstages"
                                                    FilterType="Numbers" runat="server">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                            <td style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                                                background-color: lightblue; border-width: 1px;">
                                                <asp:Label ID="lblfromstage" runat="server" Text="From" CssClass="font"></asp:Label>
                                            </td>
                                            <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                                                border-width: 1px; border-right-style: solid;">
                                                <%--<asp:DropDownList ID="ddlfromstage" runat="server"  CssClass="font" Width="100px"
                                                  AutoPostBack="True" onselectedindexchanged="ddlfromstage_SelectedIndexChanged" ></asp:DropDownList>--%>
                                                <asp:TextBox ID="tbfromstage1" runat="server" OnTextChanged="tbfromstage1_TextChanged"
                                                    MaxLength="25" placeholder="Starting Stage" CssClass="font" Height="12px" Width="110px"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="tbfromstage1_AutoCompleteExtender" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="GetListofCountries" MinimumPrefixLength="1" EnableCaching="true"
                                                    ServicePath="" TargetControlID="tbfromstage1">
                                                </asp:AutoCompleteExtender>
                                            </td>
                                            <td style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                                                background-color: lightblue; border-width: 1px;">
                                                <asp:Label ID="lblto" runat="server" Text="To" CssClass="font"></asp:Label>
                                            </td>
                                            <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                                                border-width: 1px; border-right-style: solid;">
                                                <asp:TextBox ID="tbtostage" runat="server" OnTextChanged="tbtostage_TextChanged"
                                                    MaxLength="25" placeholder="Ending Stage" CssClass="font" Height="12px" Width="110px"></asp:TextBox>
                                                <%-- <asp:DropDownList ID="ddltostage" runat="server"  CssClass="font" Width="100px"
                                                  AutoPostBack="True" onselectedindexchanged="ddltostage_SelectedIndexChanged" ></asp:DropDownList>--%>
                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="GetListofCountries" MinimumPrefixLength="1" EnableCaching="true"
                                                    ServicePath="" TargetControlID="tbtostage">
                                                </asp:AutoCompleteExtender>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnmaingoStage" runat="server" Width="32px" Text="Go" Font-Bold="True"
                                                    OnClick="btnmaingoStage_Click" Font-Names="Book Antiqua" Font-Size="Medium" />
                                            </td>
                                        </tr>
                                    </table>
                                    <div style="height: 10px;">
                                    </div>
                                    <div style="text-align: right;">
                                        <asp:Button ID="Btn_addabove" runat="server" Text="Add Above" OnClick="Btn_addabove_Click"
                                            Font-Bold="True" Font-Names="Book Antiqua" />
                                        <asp:Button ID="Btn_Addbelow" runat="server" Text="Add Below" OnClick="Btn_addbelow_Click"
                                            Font-Bold="True" Font-Names="Book Antiqua" />
                                        <asp:Button ID="Btn_Delete_Row" runat="server" Text="Delete" OnClick="Btn_Delete_Row_Click"
                                            Font-Bold="True" Font-Names="Book Antiqua" />
                                    </div>
                                    <div style="height: 10px;">
                                    </div>
                                    <center>
                                        <table class="tabl" style="top: 306px; left: 56px; width: 650px; height: 70px;">
                                            <tr>
                                                <td>
                                                    <center>
                                                        <FarPoint:FpSpread ID="FpSpreadstage" runat="server" BorderColor="Black" BorderStyle="Solid"
                                                            OnButtonCommand="FpSpreadstage_ButtonCommand" OnUpdateCommand="FpSpreadstage_UpdateCommand"
                                                            BorderWidth="1px" Height="300px" Width="768px" HorizontalScrollBarPolicy="Never"
                                                            VerticalScrollBarPolicy="Never">
                                                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                                                ButtonShadowColor="ControlDark">
                                                            </CommandBar>
                                                            <Sheets>
                                                                <FarPoint:SheetView SheetName="Sheet1">
                                                                </FarPoint:SheetView>
                                                            </Sheets>
                                                            <ClientEvents EditStopped="FpSpreadstage_ActiveCellChanged" />
                                                        </FarPoint:FpSpread>
                                                    </center>
                                                    <%--<asp:Label ID="Label3" runat="server" Text="P"></asp:Label>--%>
                                                    <asp:Label ID="Label3" runat="server" ForeColor="Blue" Text="Note:Please Enter Time"
                                                        Font-Names="MS Sans Serif" Font-Size="10pt" Visible="false" Style="top: 285px;
                                                        left: 33px; position: absolute;"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </center>
                                    <div style="height: 10px;">
                                    </div>
                                    <div>
                                        <asp:Button ID="Buttonsave" runat="server" Text="Set Return Trip" OnClick="Buttonsave_Click"
                                            Font-Bold="true" Font-Names="MS Sans Serif" Font-Size="Small" Style="top: 594px;
                                            left: 56px;" Font-Underline="False" ForeColor="Black" Width="137px" Height="21px" />
                                    </div>
                                    <div style="height: 10px;">
                                    </div>
                                    <center>
                                        <table class="tabl" style="top: 622px; left: 56px; width: 650px; border-color: Gray;
                                            border-width: thin; height: 70px;">
                                            <tr>
                                                <td>
                                                    <center>
                                                        <FarPoint:FpSpread ID="sprdMainstage" runat="server" BorderColor="Black" BorderStyle="Solid"
                                                            OnTextChanged="sprdMainstage_TextChanged" BorderWidth="1px" Height="200" Width="1046"
                                                            HorizontalScrollBarPolicy="AsNeeded" OnButtonCommand="sprdMainstage_ButtonCommand"
                                                            VerticalScrollBarPolicy="Never">
                                                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                                                ButtonShadowColor="ControlDark" ButtonType="PushButton">
                                                            </CommandBar>
                                                            <Sheets>
                                                                <FarPoint:SheetView SheetName="Sheet1">
                                                                </FarPoint:SheetView>
                                                            </Sheets>
                                                            <ClientEvents EditStopped="FpMainstages_ActiveCellChanged" />
                                                        </FarPoint:FpSpread>
                                                    </center>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:Label ID="Label2" runat="server" ForeColor="Blue" Text="Note:Please Enter Time like the following Format(Ex:07.30)"
                                            Font-Names="MS Sans Serif" Font-Size="10pt" Visible="true" Style="top: 207PX;
                                            left: 33px; position: absolute;"></asp:Label>
                                        <asp:Label ID="ErrorLabel" runat="server" ForeColor="Red" Text="" Font-Names="MS Sans Serif"
                                            Font-Size="5pt" Visible="false" Style="top: 262px; left: 525px; position: absolute;"></asp:Label>
                                    </center>
                                    <div style="height: 20px;">
                                    </div>
                                    <div style="text-align: center;">
                                        <asp:Button ID="btnnew" runat="server" Text="New" OnClick="btnnew_Click" Font-Bold="true"
                                            Font-Names="MS Sans Serif" Font-Size="Small" Font-Underline="False" ForeColor="Black"
                                            Width="60px" Height="25px" />
                                        <asp:Button ID="btnsave2" runat="server" Text="Save" OnClick="ButtonsaveRoute_Click"
                                            Font-Bold="true" Font-Names="MS Sans Serif" Font-Size="Small" Font-Underline="False"
                                            ForeColor="Black" Width="60px" Height="25px" />
                                        <asp:Button ID="Buttondelete" runat="server" Text="Delete" OnClick="Buttondelete_Click"
                                            Font-Bold="true" Font-Names="MS Sans Serif" Font-Size="Small" Font-Underline="False"
                                            ForeColor="Black" Width="70px" Height="25px" Enabled="False" />
                                    </div>
                                    <asp:Panel ID="pnlmsgboxdelete" runat="server" CssClass="modalPopup" Style="display: none;
                                        height: 100; width: 300;" DefaultButton="btnOk">
                                        <table width="100%">
                                            <tr class="topHandle">
                                                <td colspan="2" align="left" runat="server" id="tdCaption">
                                                    <asp:Label ID="lblCaption" runat="server" Font-Bold="True" Text="Confirmation" Font-Names="Book Antiqua"
                                                        Font-Size="Large"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 60px" valign="middle" align="center">
                                                    <asp:Image ID="imgInfo" runat="server" ImageUrl="~/Info-48x48.png" />
                                                </td>
                                                <td valign="middle" align="left">
                                                    <asp:Label ID="lblMessage" Text="Do You want to Delete the Record?" runat="server"
                                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="right">
                                                    <asp:Button ID="btnOk" runat="server" Text="Yes" OnClick="btnOk_Click" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" />
                                                    <asp:Button ID="btnCancel" runat="server" Text="No" OnClick="btnCancel_Click" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:HiddenField runat="server" ID="hfdelete" />
                                    <asp:ModalPopupExtender ID="mpemsgboxdelete" runat="server" TargetControlID="hfdelete"
                                        PopupControlID="pnlmsgboxdelete">
                                    </asp:ModalPopupExtender>
                                </asp:Panel>
                            </div>
                        </Content>
                    </asp:AccordionPane>
                </Panes>
            </asp:Accordion>
        </ContentTemplate>
    </asp:UpdatePanel>
    <style type="text/css">
        ody, input
        {
            font-family: Tahoma;
            font-size: 11px;
        }
        
        .modalBackground
        {
            background-color: Gray;
            filter: alpha(opacity=80);
            opacity: 0.8;
        }
        
        .modalPopup
        {
            background-color: #ffffdd;
            border-width: 1px;
            -moz-border-radius: 5px;
            border-style: solid;
            border-color: Gray;
            min-width: 250px;
            max-width: 500px;
            min-height: 100px;
            max-height: 150px;
            top: 100px;
            left: 150px;
        }
        
        .topHandle
        {
            background-color: #97bae6;
        }
        .floatr
        {
            border-style: solid;
            border-color: Black;
            border-width: thin;
            float: right;
        }
        
        
        .tabl
        {
            empty-cells: show;
            border-style: solid;
            border-color: Gray;
            border-width: thin;
            caption-side: top;
            font-family: MS Sans Serif;
            font-size: Small;
            font-style: normal;
            font-weight: normal;
        }
        .tablfont
        {
            empty-cells: show;
            border-style: solid;
            border-color: Gray;
            border-width: thin;
            caption-side: top;
            font-family: MS Sans Serif;
            font-size: Small;
            font-style: normal;
            font-weight: bold;
        }
        .tabl3
        {
            empty-cells: hide;
            border-style: solid;
            border-color: Black;
            border-width: thin;
            caption-side: top;
            font-family: MS Sans Serif;
            font-size: Small;
            font-style: normal;
            font-weight: bold;
            text-align: left;
        }
        .tabl5
        {
            border-style: solid;
            border-color: Black;
            border-width: thin;
            text-align: left;
        }
        .tabl1
        {
            empty-cells: show;
            caption-side: top;
            font-family: MS Sans Serif;
            font-size: Small;
            font-style: normal;
            font-weight: bold;
        }
        .linkbtn
        {
            border-color: White;
            border-style: none;
            background-color: transparent;
            cursor: pointer;
        }
        .HeaderSelectedCSS
        {
            color: white;
            background-color: #719DDB;
            font-weight: bold;
            font-size: medium; /* font-style:italic;  */
        }
        .HellowWorldPopup
        {
            min-width: 600px;
            min-height: 400px;
            background: white;
        }
        .ModalPopupBG
        {
            background-color: #666699;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
        
        .style2
        {
            width: 118px;
        }
        .style4
        {
            width: 43px;
        }
        .stylefp
        {
            cursor: pointer;
        }
        .style5
        {
            width: 185px;
        }
        .style55
        {
            width: 25px;
        }
        .style27
        {
            width: 25px;
        }
        .style25
        {
            width: 200px;
        }
        .style251
        {
            width: 125px;
        }
        .style6
        {
            width: 528px;
        }
        .style12
        {
            width: 200px;
        }
        .style22
        {
            width: 122px;
        }
        .style24
        {
            width: 30px;
        }
        
        .font
        {
            font-size: Small;
            font-family: MS Sans Serif;
        }
        .HeaderCSS
        {
            color: white;
            background-color: #719DDB;
            font-size: small; /* border:solid 1px salmon; */
            font-weight: bold;
            height: 10px;
        }
        .cpBody
        {
            background-color: #DCE4F9; /*font: normal 11px auto Verdana, Arial;
            border: 1px gray;               
            padding-top: 7px;
            padding-left: 4px;
            padding-right: 4px;
            padding-bottom: 4px;
            width:720;*/
        }
        .accordion
        {
            width: 300px;
        }
        
        .accordionHeader
        {
            border: 1px solid #2F4F4F;
            color: white;
            background-color: #2E4d7B;
            font-family: Arial, Sans-Serif;
            font-size: 12px;
            font-weight: bold;
            padding: 5px;
            margin-top: 5px;
            cursor: pointer;
        }
        
        .accordionHeaderSelected
        {
            border: 1px solid #2F4F4F;
            color: white;
            background-color: #5078B3;
            font-family: Arial, Sans-Serif;
            font-size: 12px;
            font-weight: bold;
            padding: 5px;
            margin-top: 5px;
            cursor: pointer;
        }
        
        .accordionContent
        {
            background-color: White;
            border: 1px dashed #2F4F4F;
            border-top: none;
            padding: 5px;
            padding-top: 10px;
        }
    </style>
</asp:Content>
