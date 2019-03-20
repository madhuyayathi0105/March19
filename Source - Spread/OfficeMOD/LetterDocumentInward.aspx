<%@ Page Title="" Language="C#" MasterPageFile="~/OfficeMOD/OfficeSubSiteMaster.master" AutoEventWireup="true" CodeFile="LetterDocumentInward.aspx.cs" Inherits="LetterDocumentInward" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1">
    <title></title>
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <style type="text/css">
        .maindivstylesize
        {
            height: 1300px;
            width: 1000px;
        }
        
        .textboxchng
{
    border: 1px solid #c4c4c4;
   
    padding: 4px 4px 4px 4px;
    border-radius: 4px;
    -moz-border-radius: 4px;
    -webkit-border-radius: 4px;
    box-shadow: 0px 0px 8px #d9d9d9;
    -moz-box-shadow: 0px 0px 8px #d9d9d9;
    -webkit-box-shadow: 0px 0px 8px #d9d9d9;
}

    </style>
</head>
<body>
    <script type="text/javascript">

        function change1(id) {
            var value1 = id.value;
            if (value1.trim().toUpperCase() == "OTHERS") {
                var idval = document.getElementById("<%=txt_docpriv.ClientID %>");
                idval.style.display = "block";
            }
            else {
                var idval = document.getElementById("<%=txt_docpriv.ClientID %>");
                idval.style.display = "none";
            }
        }

        function change2(id) {
            var value1 = id.value;
            if (value1.trim().toUpperCase() == "OTHERS") {
                var idval = document.getElementById("<%=txt_delivery.ClientID %>");
                idval.style.display = "block";
            }
            else {
                var idval = document.getElementById("<%=txt_delivery.ClientID %>");
                idval.style.display = "none";
            }
        }

        function change3() {
            var idval = document.getElementById("<%=txt_to1.ClientID %>");
            idval.style.display = "block";
            return false;
        }


        function change31() {
            var idval = document.getElementById("<%=txt_to1.ClientID %>");
            idval.style.display = "none";
            document.getElementById('<%=txt_to1.ClientID %>').value = "";

            return false;
        }

        function change4() {
            var idval = document.getElementById("<%=txt_cc1.ClientID %>");
            idval.style.display = "block";

            return false;
        }

        function change41() {
            var idval = document.getElementById("<%=txt_cc1.ClientID %>");
            idval.style.display = "none";
            document.getElementById('<%=txt_cc1.ClientID %>').value = "";
            return false;
        }

        function change5() {
            var idval = document.getElementById("<%=txt_indiv1.ClientID %>");
            idval.style.display = "block";
            return false;
        }

        function change51() {
            var idval = document.getElementById("<%=txt_indiv1.ClientID %>");
            idval.style.display = "none";
            document.getElementById('<%=txt_indiv1.ClientID %>').value = "";
            return false;
        }

        function change6() {
            var idval = document.getElementById("<%=txt_cc2.ClientID %>");
            idval.style.display = "block";
            return false;
        }

        function change61() {
            var idval = document.getElementById("<%=txt_cc2.ClientID %>");
            idval.style.display = "none";
            document.getElementById('<%=txt_cc2.ClientID %>').value = "";
            return false;
        }

        function checkchange1(id) {
            if (document.getElementById('<%=cb_dept.ClientID %>').checked == true) {
                var idval = document.getElementById("<%=div_dept.ClientID %>");
                idval.style.display = "block";
                return false;
            }
            if (document.getElementById('<%=cb_dept.ClientID %>').checked == false) {
                var idval = document.getElementById("<%=div_dept.ClientID %>");
                idval.style.display = "none";
                return false;
            }

        }

        function checkchange2() {
            if (document.getElementById('<%=cb_individual.ClientID %>').checked == true) {
                var idval = document.getElementById("<%=div_indiv.ClientID %>");
                idval.style.display = "block";
                return false;
            }
            else {
                var idval = document.getElementById("<%=div_indiv.ClientID %>");
                idval.style.display = "none";
                return false;
            }
        }

        function rbcurrentold() {
            rbchange();
            div_stud_clear();
            var jai = "C";
            newcheckget(jai);
        }


        function rbcurrentold1() {
            rbchange1();
            div_stud_clear();
            var jai = "O";
            newcheckget(jai);
        }

        function rbcurrentparent() {
            rbchang();

            parent_div_clear();
            var jai = "C";
            newcheckget(jai);
        }

        function rbcurrentparent1() {
            rbchang1();

            parent_div_clear();
            var jai = "O";
            newcheckget(jai);
        }

        function rbcurrentstaff() {
            staff_div_clear();
            var jai = "C";
            newcheckget(jai);
        }

        function rbcurrentstaff1() {
            staff_div_clear();
            var jai = "O";
            newcheckget(jai);
        }

        function rbchange() {
            if (document.getElementById('<%=rdo_current.ClientID %>').checked == true) {
//                var idval = document.getElementById("<%=lbl_roll.ClientID %>");
//                idval.style.display = "block";
//                var id = document.getElementById("<%=lbl_reg.ClientID %>");
//                id.style.display = "none";
                return false;
            }
        }





        function rbchange1() {
            if (document.getElementById('<%=rdo_old.ClientID %>').checked == true) {
//                var idval = document.getElementById("<%=lbl_reg.ClientID %>");
//                idval.style.display = "block";
//                var id = document.getElementById("<%=lbl_roll.ClientID %>");
//                id.style.display = "none";
                return false;
            }

        }


        function rbchang() {
            if (document.getElementById('<%=rdo_parent_current.ClientID %>').checked == true) {
                var idval = document.getElementById("<%=lbl_rollno.ClientID %>");
                idval.style.display = "block";
                var id = document.getElementById("<%=lbl_par_reg.ClientID %>");
                id.style.display = "none";
                return false;
            }
        }


        function rbchang1() {
            if (document.getElementById('<%=rdo_parent_old.ClientID %>').checked == true) {
                var idval = document.getElementById("<%=lbl_par_reg.ClientID %>");
                idval.style.display = "block";
                var id = document.getElementById("<%=lbl_rollno.ClientID %>");
                id.style.display = "none";
                return false;
            }

        }


        // ********************from /to stud**************

        function rbinfromto() {
            if (document.getElementById('<%=rb_in.ClientID %>').checked == true) {
                var idval = document.getElementById("<%=lbl_from.ClientID %>");
                idval.style.display = "block";

                var idval1 = document.getElementById("<%=lbl_to.ClientID %>");
                idval1.style.display = "block";

                var id = document.getElementById("<%=lbl_chn_to.ClientID %>");
                id.style.display = "none";

                var id1 = document.getElementById("<%=lbl_chng_frm.ClientID %>");
                id1.style.display = "none";
//                rdo_current.checked = true;
//                cb_dept.checked = true;
//                cb_individual.checked = false;
                document.getElementById('<%=rdo_current.ClientID %>').checked = true;
                document.getElementById('<%=cb_dept.ClientID %>').checked = true;
                document.getElementById('<%=cb_individual.ClientID %>').checked = false;

                var idd = document.getElementById("<%=div_dept.ClientID %>");
                idd.style.display = "block";
                var idd1 = document.getElementById("<%=div_indiv.ClientID %>");
                idd1.style.display = "none";
                return false;
            }
        }


        function rbinfromto1() {
            if (document.getElementById('<%=rb_out.ClientID %>').checked == true) {
                var idval = document.getElementById("<%=lbl_chn_to.ClientID %>");
                idval.style.display = "block";


                var id1 = document.getElementById("<%=lbl_chng_frm.ClientID %>");
                id1.style.display = "block";

                var id = document.getElementById("<%=lbl_from.ClientID %>");
                id.style.display = "none";

                var idval1 = document.getElementById("<%=lbl_to.ClientID %>");
                idval1.style.display = "none";
//                rdo_current.checked = true;
//                cb_dept.checked = true;
//                cb_individual.checked = false;
                document.getElementById('<%=rdo_current.ClientID %>').checked = true;
                document.getElementById('<%=cb_dept.ClientID %>').checked = true;
                document.getElementById('<%=cb_individual.ClientID %>').checked = false;

                var idd = document.getElementById("<%=div_dept.ClientID %>");
                idd.style.display = "block";
                var idd1 = document.getElementById("<%=div_indiv.ClientID %>");
                idd1.style.display = "none";
                return false;
            }
        }


        ///*************************************************from to staff*************


        function rbinfromto_staff() {
            if (document.getElementById('<%=rb_stf_in.ClientID %>').checked == true) {
                document.getElementById("<%=lbl_stf_delivry.ClientID %>").innerHTML="Received Mode"
                
                
                
               
                var idval = document.getElementById("<%=lbl_stud_from.ClientID %>");
                idval.style.display = "block";
               

                var idval1 = document.getElementById("<%=lbl_stf_to.ClientID %>");
                idval1.style.display = "block";

                var id = document.getElementById("<%=lbl_chng_to_staff.ClientID %>");
                id.style.display = "none";

                var id1 = document.getElementById("<%=lbl_chng_from_staff.ClientID %>");
                id1.style.display = "none";
                document.getElementById('<%=rb_current.ClientID %>').checked = true;
                document.getElementById('<%=cb_stf_dept.ClientID %>').checked = true;

                var idd = document.getElementById("<%=div_stf_dept.ClientID %>");
                idd.style.display = "block";
                var idd1 = document.getElementById("<%=div_stf_indi.ClientID %>");
                idd1.style.display = "none";
                document.getElementById('<%=cb_stf_indi.ClientID %>').checked = false;
                return false;
            }
        }


        function rbinfromto1_staff() {
            if (document.getElementById('<%=rb_stf_out.ClientID %>').checked == true) {

                document.getElementById("<%=lbl_stf_delivry.ClientID %>").innerHTML = "Delivery Mode"

                var idval = document.getElementById("<%=lbl_chng_to_staff.ClientID %>");
                idval.style.display = "block";


                var id1 = document.getElementById("<%=lbl_chng_from_staff.ClientID %>");
                id1.style.display = "block";

                var id = document.getElementById("<%=lbl_stud_from.ClientID %>");
                id.style.display = "none";

                var idval1 = document.getElementById("<%=lbl_stf_to.ClientID %>");
                idval1.style.display = "none";

                document.getElementById('<%=rb_current.ClientID %>').checked = true;
                document.getElementById('<%=cb_stf_dept.ClientID %>').checked = true;
                var idd = document.getElementById("<%=div_stf_dept.ClientID %>");
                idd.style.display = "block";
                var idd1 = document.getElementById("<%=div_stf_indi.ClientID %>");
                idd1.style.display = "none";
                document.getElementById('<%=cb_stf_indi.ClientID %>').checked = false;
                return false;
            }
        }



        ///**************from -to parent **********************

        function rbinfromto_par() {
            if (document.getElementById('<%=rb_par_in.ClientID %>').checked == true) {
                var idval = document.getElementById("<%=lbl_parfrom.ClientID %>");
                idval.style.display = "block";

                var idval1 = document.getElementById("<%=lbl_parto.ClientID %>");
                idval1.style.display = "block";

                var id = document.getElementById("<%=lbl_chng_to_par.ClientID %>");
                id.style.display = "none";

                var id1 = document.getElementById("<%=lbl_chng_from_par.ClientID %>");
                id1.style.display = "none";
               // rdo_parent_current.checked = true;
                document.getElementById('<%=rdo_parent_current.ClientID %>').checked = true;
                document.getElementById('<%=cb_par_dept.ClientID %>').checked = true;
                document.getElementById('<%=cb_par_indi.ClientID %>').checked = false;
                document.getElementById('<%=cb_par_stud.ClientID %>').checked = false;
                var idd = document.getElementById("<%=div_parnt_dept.ClientID %>");
                idd.style.display = "block";
                var idd1 = document.getElementById("<%=div_parnt_indi.ClientID %>");
                idd1.style.display = "none";
                var idd2 = document.getElementById("<%=div_par_stud.ClientID %>");
                idd2.style.display = "none";
                return false;
            }
        }


        function rbinfromto1_par() {
            if (document.getElementById('<%=rb_par_out.ClientID %>').checked == true) {
                var idval = document.getElementById("<%=lbl_chng_from_par.ClientID %>");
                idval.style.display = "block";


                var id1 = document.getElementById("<%=lbl_chng_to_par.ClientID %>");
                id1.style.display = "block";

                var id = document.getElementById("<%=lbl_parfrom.ClientID %>");
                id.style.display = "none";

                var idval1 = document.getElementById("<%=lbl_parto.ClientID %>");
                idval1.style.display = "none";
                document.getElementById('<%=rdo_parent_current.ClientID %>').checked = true;
                document.getElementById('<%=cb_par_dept.ClientID %>').checked = true;
                document.getElementById('<%=cb_par_indi.ClientID %>').checked = false;
                document.getElementById('<%=cb_par_stud.ClientID %>').checked = false;
                var idd = document.getElementById("<%=div_parnt_dept.ClientID %>");
                idd.style.display = "block";
                var idd1 = document.getElementById("<%=div_parnt_indi.ClientID %>");
                idd1.style.display = "none";
                var idd2 = document.getElementById("<%=div_par_stud.ClientID %>");
                idd2.style.display = "none";
                return false;
            }
        }



        //**************from to company********************

        function rbinfromto_comp() {
            if (document.getElementById('<%=rb_comp_in.ClientID %>').checked == true) {

                document.getElementById("<%=lbl_com_delivry.ClientID %>").innerHTML = "Received Mode"




                var idval = document.getElementById("<%=lbl_com_from.ClientID %>");
                idval.style.display = "block";

                var idval1 = document.getElementById("<%=lbl_com_to.ClientID %>");
                idval1.style.display = "block";

                var id = document.getElementById("<%=lbl_chng_from_comp.ClientID %>");
                id.style.display = "none";

                var id1 = document.getElementById("<%=lbl_chng_to_com.ClientID %>");
                id1.style.display = "none";
                document.getElementById('<%=cb_com_dept.ClientID %>').checked = true;
                document.getElementById('<%=cb_com_indi.ClientID %>').checked = false;
                var idd = document.getElementById("<%=div_comp_dept.ClientID %>");
                idd.style.display = "block";
                var idd1 = document.getElementById("<%=div_comp_indi.ClientID %>");
                idd1.style.display = "none";
                return false;
            }
        }


        function rbinfromto1_comp() {
            if (document.getElementById('<%=rb_comp_out.ClientID %>').checked == true) {
                document.getElementById("<%=lbl_com_delivry.ClientID %>").innerHTML = "Delivery Mode"
                var idval = document.getElementById("<%=lbl_chng_to_com.ClientID %>");
                idval.style.display = "block";


                var id1 = document.getElementById("<%=lbl_chng_from_comp.ClientID %>");
                id1.style.display = "block";

                var id = document.getElementById("<%=lbl_com_from.ClientID %>");
                id.style.display = "none";

                var idval1 = document.getElementById("<%=lbl_com_to.ClientID %>");
                idval1.style.display = "none";
                document.getElementById('<%=cb_com_dept.ClientID %>').checked = true;
                document.getElementById('<%=cb_com_indi.ClientID %>').checked = false;
                var idd = document.getElementById("<%=div_comp_dept.ClientID %>");
                idd.style.display = "block";
                var idd1 = document.getElementById("<%=div_comp_indi.ClientID %>");
                idd1.style.display = "none";
                return false;
            }
        }
        //**************** from to individual ***************

        function rbinfromto_indi() {
            if (document.getElementById('<%=rb_indiv_in.ClientID %>').checked == true) {
                var idval = document.getElementById("<%=lbl_ind_from.ClientID %>");
                idval.style.display = "block";

                var idval1 = document.getElementById("<%=lbl_ind_to.ClientID %>");
                idval1.style.display = "block";

                var id = document.getElementById("<%=lbl_chng_from_indi.ClientID %>");
                id.style.display = "none";

                var id1 = document.getElementById("<%=lbl_chng_to_indi.ClientID %>");
                id1.style.display = "none";
                document.getElementById('<%=cb_ind_dept.ClientID %>').checked = true;
                document.getElementById('<%=cb_ind_indi.ClientID %>').checked = false;
                var idd = document.getElementById("<%=div_indiv_dept.ClientID %>");
                idd.style.display = "block";
                var idd1 = document.getElementById("<%=div_indiv_indi.ClientID %>");
                idd1.style.display = "none";
                return false;
            }
        }


        function rbinfromto1_indi() {
            if (document.getElementById('<%=rb_indiv_out.ClientID %>').checked == true) {
                var idval = document.getElementById("<%=lbl_chng_from_indi.ClientID %>");
                idval.style.display = "block";


                var id1 = document.getElementById("<%=lbl_chng_to_indi.ClientID %>");
                id1.style.display = "block";

                var id = document.getElementById("<%=lbl_ind_from.ClientID %>");
                id.style.display = "none";

                var idval1 = document.getElementById("<%=lbl_ind_to.ClientID %>");
                idval1.style.display = "none";
                document.getElementById('<%=cb_ind_dept.ClientID %>').checked = true;
                document.getElementById('<%=cb_ind_indi.ClientID %>').checked = false;
                var idd = document.getElementById("<%=div_indiv_dept.ClientID %>");
                idd.style.display = "block";
                var idd1 = document.getElementById("<%=div_indiv_indi.ClientID %>");
                idd1.style.display = "none";
                return false;
            }
        }

        //*********** from to mag********************


        function rbinfromto_mag() {
            if (document.getElementById('<%=rb_magz_in.ClientID %>').checked == true) {
                var idval = document.getElementById("<%=lbl_magz_from.ClientID %>");
                idval.style.display = "block";

                var idval1 = document.getElementById("<%=lbl_magz_to.ClientID %>");
                idval1.style.display = "block";

                var id = document.getElementById("<%=lbl_chng_to_mag.ClientID %>");
                id.style.display = "none";

                var id1 = document.getElementById("<%=lbl_chng_from_mag.ClientID %>");
                id1.style.display = "none";
                document.getElementById('<%=cb_magz_dept.ClientID %>').checked = true;
                document.getElementById('<%=cb_magz_indi.ClientID %>').checked = false;
                var idd = document.getElementById("<%=div_magz_dept.ClientID %>");
                idd.style.display = "block";
                var idd1 = document.getElementById("<%=div_magz_indi.ClientID %>");
                idd1.style.display = "none";
                return false;
            }
        }


        function rbinfromto1_mag() {
            if (document.getElementById('<%=rb_magz_out.ClientID %>').checked == true) {
                var idval = document.getElementById("<%=lbl_chng_from_mag.ClientID %>");
                idval.style.display = "block";


                var id1 = document.getElementById("<%=lbl_chng_to_mag.ClientID %>");
                id1.style.display = "block";

                var id = document.getElementById("<%=lbl_magz_from.ClientID %>");
                id.style.display = "none";

                var idval1 = document.getElementById("<%=lbl_magz_to.ClientID %>");
                idval1.style.display = "none";
                document.getElementById('<%=cb_magz_dept.ClientID %>').checked = true;
                document.getElementById('<%=cb_magz_indi.ClientID %>').checked = false;
                var idd = document.getElementById("<%=div_magz_dept.ClientID %>");
                idd.style.display = "block";
                var idd1 = document.getElementById("<%=div_magz_indi.ClientID %>");
                idd1.style.display = "none";
                return false;
            }
        }
        ///**********from to oth***********



        function rbinfromto_oth() {
            if (document.getElementById('<%=rb_othr_in.ClientID %>').checked == true) {
                document.getElementById("<%=lbl_othr_delivry.ClientID %>").innerHTML = "Received Mode"
                var idval = document.getElementById("<%=lbl_othr_from.ClientID %>");
                idval.style.display = "block";

                var idval1 = document.getElementById("<%=lbl_othr_to.ClientID %>");
                idval1.style.display = "block";

                var id = document.getElementById("<%=lbl_chng_from_othr.ClientID %>");
                id.style.display = "none";

                var id1 = document.getElementById("<%=lbl_chng_to_othr.ClientID %>");
                id1.style.display = "none";
                document.getElementById('<%=cb_othr_dept.ClientID %>').checked = true;
                document.getElementById('<%=cb_othr_indi.ClientID %>').checked = false;
                var idd = document.getElementById("<%=div_other_dept.ClientID %>");
                idd.style.display = "block";
                var idd1 = document.getElementById("<%=div_other_indi.ClientID %>");
                idd1.style.display = "none";
                return false;
            }
        }


        function rbinfromto1_oth() {
            if (document.getElementById('<%=rb_othr_out.ClientID %>').checked == true) {
                document.getElementById("<%=lbl_othr_delivry.ClientID %>").innerHTML = "Delivery Mode"
                var idval = document.getElementById("<%=lbl_chng_to_othr.ClientID %>");
                idval.style.display = "block";


                var id1 = document.getElementById("<%=lbl_chng_from_othr.ClientID %>");
                id1.style.display = "block";

                var id = document.getElementById("<%=lbl_othr_from.ClientID %>");
                id.style.display = "none";

                var idval1 = document.getElementById("<%=lbl_othr_to.ClientID %>");
                idval1.style.display = "none";
                document.getElementById('<%=cb_othr_dept.ClientID %>').checked = true;
                document.getElementById('<%=cb_othr_indi.ClientID %>').checked = false;
                var idd = document.getElementById("<%=div_other_dept.ClientID %>");
                idd.style.display = "block";
                var idd1 = document.getElementById("<%=div_other_indi.ClientID %>");
                idd1.style.display = "none";
                return false;
            }
        }



        //************** end****************

        function browse_change() {
            var idval = document.getElementById("<%=popwindow1.ClientID %>");
            idval.style.display = "block";
            return false;
        }

        function cleardivstud() {
            document.getElementById('<%=txt_roll.ClientID %>').innerHTML = "";

            return false;

        }


        function newcheckget(txt) {
            $.ajax({
                type: "POST",
                url: "LetterDocumentInward.aspx/getDatanewcheck",
                data: '{Roll_No: "' + txt + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                },
                failure: function (response) {
                    alert(response);
                }
            });
        }


        function get(txt) {//delsi2602

            $.ajax({
                type: "POST",
                url: "LetterDocumentInward.aspx/getData",
                data: '{Roll_No: "' + txt + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    bindmthd(response.d);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }


        function getname(txt1) {

            $.ajax({
                type: "POST",
                url: "LetterDocumentInward.aspx/getData1",
                data: '{Name: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    bindmthd(response.d);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function getreg(txt) {

            $.ajax({
                type: "POST",
                url: "LetterDocumentInward.aspx/getDatareg",
                data: '{Reg_No: "' + txt + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    bindmthd(response.d);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }


        function bindmthd(Employees) {

            var rno = Employees[0].RollNo;
            document.getElementById('<%=txt_roll.ClientID %>').value = rno;

            var name = Employees[0].Name;
            document.getElementById('<%=txt_name.ClientID %>').value = name;

            var batch = Employees[0].Batchyear;
            document.getElementById('<%=txt_batch.ClientID %>').value = batch;

            var degree = Employees[0].Degree;
            document.getElementById('<%=txt_degree.ClientID %>').value = degree;

            var dept = Employees[0].Department;
            document.getElementById('<%=txt_department.ClientID %>').value = dept;

            var sec = Employees[0].section;
            document.getElementById('<%=txt_section.ClientID %>').value = sec;

            var sem = Employees[0].Semester;
            document.getElementById('<%=txt_sem.ClientID %>').value = sem;

            var stud_type = Employees[0].StudentType;
            document.getElementById('<%=txt_studtype.ClientID %>').value = stud_type;

            var addrs = Employees[0].Address;
            document.getElementById('<%=txt_addrs.ClientID %>').value = addrs;

            var street = Employees[0].Street;
            document.getElementById('<%=txt_street.ClientID %>').value = street;

            var city = Employees[0].City;
            document.getElementById('<%=txt_city.ClientID %>').value = city;

            var pin = Employees[0].Pin;
            document.getElementById('<%=txt_pin_stud.ClientID %>').value = pin;

            var state = Employees[0].State;
            document.getElementById('<%=txt_state.ClientID %>').value = state;

            var mob = Employees[0].Phone;
            document.getElementById('<%=txt_pho.ClientID %>').value = mob;

            var mail = Employees[0].MailId;
            document.getElementById('<%=txt_mailid.ClientID %>').value = mail;

        }
        //        .**************************staff***************************

        function changestaff1(id) {
            var value1 = id.value;
            if (value1.trim().toUpperCase() == "OTHERS") {
                var idval = document.getElementById("<%=txt_doc.ClientID %>");
                idval.style.display = "block";
            }
            else {
                var idval = document.getElementById("<%=txt_doc.ClientID %>");
                idval.style.display = "none";
            }
        }


        function changebank1(id) {
            var value1 = id.value;
            if (value1.trim().toUpperCase() == "OTHERS") {
                var idval = document.getElementById("<%=txt_bank.ClientID %>");
                idval.style.display = "block";
            }
            else {
                var idval = document.getElementById("<%=txt_bank.ClientID %>");
                idval.style.display = "none";
            }
        }


        function changebankcomp(id) {
            var value1 = id.value;
            if (value1.trim().toUpperCase() == "OTHERS") {
                var idval = document.getElementById("<%=txt_bank_company.ClientID %>");
                idval.style.display = "block";
            }
            else {
                var idval = document.getElementById("<%=txt_bank_company.ClientID %>");
                idval.style.display = "none";
            }
        }

        function changebankothers(id) {
            var value1 = id.value;
            if (value1.trim().toUpperCase() == "OTHERS") {
                var idval = document.getElementById("<%=txt_others_bank.ClientID %>");
                idval.style.display = "block";
            }
            else {
                var idval = document.getElementById("<%=txt_others_bank.ClientID %>");
                idval.style.display = "none";
            }
        }


        function change2staff(id) {
            var value1 = id.value;
            if (value1.trim().toUpperCase() == "OTHERS") {
                var idval = document.getElementById("<%=txt_deliv.ClientID %>");
                idval.style.display = "block";
            }
            else {
                var idval = document.getElementById("<%=txt_deliv.ClientID %>");
                idval.style.display = "none";
            }
        }
        function staff_change() {
            var idval = document.getElementById("<%=txt_stf_depto1.ClientID %>");
            idval.style.display = "block";
            return false;
        }

        function staff_change0() {
            var idval = document.getElementById("<%=txt_stf_depto1.ClientID %>");
            document.getElementById('<%=txt_stf_depto1.ClientID %>').value = "";
            idval.style.display = "none";

            return false;
        }

        function staff_change1() {
            var idval = document.getElementById("<%=txt_stf_cc1.ClientID %>");
            idval.style.display = "block";
            return false;
        }
        function staff_change11() {
            var idval = document.getElementById("<%=txt_stf_cc1.ClientID %>");
            idval.style.display = "none";
            document.getElementById('<%=txt_stf_cc1.ClientID %>').value = "";
            return false;
        }

        function staff_change2() {
            var idval = document.getElementById("<%=txt_stf_indito1.ClientID %>");
            idval.style.display = "block";
            return false;
        }

        function staff_change22() {
            var idval = document.getElementById("<%=txt_stf_indito1.ClientID %>");
            idval.style.display = "none";
            document.getElementById('<%=txt_stf_indito1.ClientID %>').value = "";
            return false;
        }

        function staff_change3() {
            var idval = document.getElementById("<%=txt_stf_indicc1.ClientID %>");
            idval.style.display = "block";
            return false;
        }

        function staff_change33() {
            var idval = document.getElementById("<%=txt_stf_indicc1.ClientID %>");
            idval.style.display = "none";
            document.getElementById('<%=txt_stf_indicc1.ClientID %>').value = "";
            return false;
        }

        function staff_checkchange1() {
            if (document.getElementById('<%=cb_stf_dept.ClientID %>').checked == true) {
                var idval = document.getElementById("<%=div_stf_dept.ClientID %>");
                idval.style.display = "block";
                return false;
            }
            else {
                var idval = document.getElementById("<%=div_stf_dept.ClientID %>");
                idval.style.display = "none";
                return false;
            }
        }
        function staff_checkchange2() {
            if (document.getElementById('<%=cb_stf_indi.ClientID %>').checked == true) {
                var idval = document.getElementById("<%=div_stf_indi.ClientID %>");
                idval.style.display = "block";
                return false;
            }
            else {
                var idval = document.getElementById("<%=div_stf_indi.ClientID %>");
                idval.style.display = "none";
                return false;
            }
        }
        function myFunction(x) {
            x.style.borderColor = "#c4c4c4";
        }

        function getstaff(txt1) {

            $.ajax({
                type: "POST",
                url: "LetterDocumentInward.aspx/getstaffdetail",
                data: '{Name: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    bindmthdstaff(response.d);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function getstaffcode(txt1) {

            $.ajax({
                type: "POST",
                url: "LetterDocumentInward.aspx/getstaffcodedetail",
                data: '{Name: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    bindmthdstaff(response.d);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }

        function getstaff1(txt2) {

            $.ajax({
                type: "POST",
                url: "LetterDocumentInward.aspx/getstaffdetail1",
                data: '{staffcode: "' + txt2 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    bindmthdstaff(response.d);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }

        function bindmthdstaff(Employees) {

            var name = Employees[0].Name;
            document.getElementById('<%=txt_name1.ClientID %>').value = name;

            var code = Employees[0].Code;
            document.getElementById('<%=txt_stf_code.ClientID %>').value = code;

            var dept = Employees[0].Department;
            document.getElementById('<%=txt_dept.ClientID %>').value = dept;

            var desgn = Employees[0].Designation;
            document.getElementById('<%=txt_desigtn.ClientID %>').value = desgn;

            var stafftype = Employees[0].StaffType;
            document.getElementById('<%=txt_stafftype.ClientID %>').value = stafftype;

            var addrs = Employees[0].Address;
            document.getElementById('<%=txt_stf_add.ClientID %>').value = addrs;

            var street = Employees[0].Street;
            document.getElementById('<%=txt_stf_str.ClientID %>').value = street;

            var city = Employees[0].City;
            document.getElementById('<%=txt_stf_city.ClientID %>').value = city;

            var pin = Employees[0].Pin;
            document.getElementById('<%=txt_stf_pin.ClientID %>').value = pin;

            var state = Employees[0].State;
            document.getElementById('<%=txt_stf_state.ClientID %>').value = state;

            var phn = Employees[0].Phone;
            document.getElementById('<%=txt_stf_ph.ClientID %>').value = phn;

            var mail = Employees[0].MailId;
            document.getElementById('<%=txt_stf_mail.ClientID %>').value = mail;

            var clear = Employees[0].clr;
            if (clear == "1") {
                document.getElementById('<%=txt_name1.ClientID %>').value = "";
            }


        }

        //  *********************************......parent.....***************************************

        function getparent(txt1) {

            $.ajax({
                type: "POST",
                url: "LetterDocumentInward.aspx/parentdetail",
                data: '{Name: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    bindmthdparent(response.d);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }

        function bindmthdparent(Employees) {

            var rno = Employees[0].RollNo;
            document.getElementById('<%=txt_rollno.ClientID %>').value = rno;

            var name = Employees[0].Name;
            document.getElementById('<%=txt_studname.ClientID %>').value = name;

            var pname = Employees[0].Parent;
            document.getElementById('<%=txt_fname.ClientID %>').value = pname;

            var mob = Employees[0].Mob;
            document.getElementById('<%=txt_mob.ClientID %>').value = mob;

            var addrs = Employees[0].Address;
            document.getElementById('<%=txt_addr.ClientID %>').value = addrs;

            var street = Employees[0].Street;
            document.getElementById('<%=txt_street2.ClientID %>').value = street;

            var city = Employees[0].City;
            document.getElementById('<%=txt_par_city.ClientID %>').value = city;

            var state = Employees[0].State;
            document.getElementById('<%=txt_par_state.ClientID %>').value = state;

            var mob1 = Employees[0].Phone;
            document.getElementById('<%=txt_par_phno.ClientID %>').value = mob1;

            var mail = Employees[0].MailId;
            document.getElementById('<%=txt_par_mail.ClientID %>').value = mail;




        }
        function getparent1(txt1) {
            $.ajax({
                type: "POST",
                url: "LetterDocumentInward.aspx/parentdetail1",
                data: '{Roll_No: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    bindmthdparent1(response.d);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }


        function bindmthdparent1(Employees) {

            var rno = Employees[0].RollNo;
            document.getElementById('<%=txt_rollno.ClientID %>').value = rno;

            var name = Employees[0].Name;
            document.getElementById('<%=txt_studname.ClientID %>').value = name;

            var pname = Employees[0].Parent;
            document.getElementById('<%=txt_fname.ClientID %>').value = pname;

            var mob = Employees[0].Mob;
            document.getElementById('<%=txt_mob.ClientID %>').value = mob;

            var addrs = Employees[0].Address;
            document.getElementById('<%=txt_addr.ClientID %>').value = addrs;

            var street = Employees[0].Street;
            document.getElementById('<%=txt_street2.ClientID %>').value = street;

            var city = Employees[0].City;
            document.getElementById('<%=txt_par_city.ClientID %>').value = city;

            var state = Employees[0].State;
            document.getElementById('<%=txt_par_state.ClientID %>').value = state;

            var mob1 = Employees[0].Phone;
            document.getElementById('<%=txt_par_phno.ClientID %>').value = mob1;

            var mail = Employees[0].MailId;
            document.getElementById('<%=txt_par_mail.ClientID %>').value = mail;


        }


        function getparent2(txt1) {
            $.ajax({
                type: "POST",
                url: "LetterDocumentInward.aspx/parentdetailname",
                data: '{ParentName: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    bindmthdparent2(response.d);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }

        function bindmthdparent2(Employees) {

            var rno = Employees[0].RollNo;
            document.getElementById('<%=txt_rollno.ClientID %>').value = rno;

            var name = Employees[0].Name;
            document.getElementById('<%=txt_studname.ClientID %>').value = name;

            var pname = Employees[0].Parent;
            document.getElementById('<%=txt_fname.ClientID %>').value = pname;

            var mob = Employees[0].Mob;
            document.getElementById('<%=txt_mob.ClientID %>').value = mob;

            var addrs = Employees[0].Address;
            document.getElementById('<%=txt_addr.ClientID %>').value = addrs;

            var street = Employees[0].Street;
            document.getElementById('<%=txt_street2.ClientID %>').value = street;

            var city = Employees[0].City;
            document.getElementById('<%=txt_par_city.ClientID %>').value = city;

            var state = Employees[0].State;
            document.getElementById('<%=txt_par_state.ClientID %>').value = state;

            var mob1 = Employees[0].Phone;
            document.getElementById('<%=txt_par_phno.ClientID %>').value = mob1;

            var mail = Employees[0].MailId;
            document.getElementById('<%=txt_par_mail.ClientID %>').value = mail;




        }

        function getparent3(txt1) {
            $.ajax({
                type: "POST",
                url: "LetterDocumentInward.aspx/parentdetailmob",
                data: '{Parentmob: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    bindmthdparent3(response.d);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }

        function bindmthdparent3(Employees) {

            var rno = Employees[0].RollNo;
            document.getElementById('<%=txt_rollno.ClientID %>').value = rno;

            var name = Employees[0].Name;
            document.getElementById('<%=txt_studname.ClientID %>').value = name;

            var pname = Employees[0].Parent;
            document.getElementById('<%=txt_fname.ClientID %>').value = pname;

            var mob = Employees[0].Mob;
            document.getElementById('<%=txt_mob.ClientID %>').value = mob;

            var addrs = Employees[0].Address;
            document.getElementById('<%=txt_addr.ClientID %>').value = addrs;

            var street = Employees[0].Street;
            document.getElementById('<%=txt_street2.ClientID %>').value = street;

            var city = Employees[0].City;
            document.getElementById('<%=txt_par_city.ClientID %>').value = city;

            var pin = Employees[0].Pin;
            document.getElementById('<%=txt_par_pin.ClientID %>').value = pin;


            var state = Employees[0].State;
            document.getElementById('<%=txt_par_state.ClientID %>').value = state;

            var mob1 = Employees[0].Phone;
            document.getElementById('<%=txt_par_phno.ClientID %>').value = mob1;

            var mail = Employees[0].MailId;
            document.getElementById('<%=txt_par_mail.ClientID %>').value = mail;

        }

        function change1parent(id) {
            var value1 = id.value;
            if (value1.trim().toUpperCase() == "OTHERS") {
                var idval = document.getElementById("<%=txt_par_doc1.ClientID %>");
                idval.style.display = "block";
            }
            else {
                var idval = document.getElementById("<%=txt_par_doc1.ClientID %>");
                idval.style.display = "none";
            }
        }

        function change2parent(id) {
            var value1 = id.value;
            if (value1.trim().toUpperCase() == "OTHERS") {
                var idval = document.getElementById("<%=txt_par_deliv1.ClientID %>");
                idval.style.display = "block";
            }
            else {
                var idval = document.getElementById("<%=txt_par_deliv1.ClientID %>");
                idval.style.display = "none";
            }
        }

        function parnt_change() {
            var idval = document.getElementById("<%=txt_par_depto1.ClientID %>");
            idval.style.display = "block";
            return false;
        }

        function parnt_change0() {
            var idval = document.getElementById("<%=txt_par_depto1.ClientID %>");
            idval.style.display = "none";
            document.getElementById('<%=txt_par_depto1.ClientID %>').value = "";
            return false;
        }

        function parnt_change1() {
            var idval = document.getElementById("<%=txt_par_cc1.ClientID %>");
            idval.style.display = "block";
            return false;
        }

        function parnt_change11() {
            var idval = document.getElementById("<%=txt_par_cc1.ClientID %>");
            idval.style.display = "none";
            document.getElementById('<%=txt_par_cc1.ClientID %>').value = "";
            return false;
        }

        function parnt_change2() {
            var idval = document.getElementById("<%=txt_par_indi1.ClientID %>");
            idval.style.display = "block";
            return false;
        }

        function parnt_change22() {
            var idval = document.getElementById("<%=txt_par_indi1.ClientID %>");
            idval.style.display = "none";
            document.getElementById('<%=txt_par_indi1.ClientID %>').value = "";
            return false;
        }

        function parnt_change3() {
            var idval = document.getElementById("<%=txt_par_cc3.ClientID %>");
            idval.style.display = "block";
            return false;
        }

        function parnt_change33() {
            var idval = document.getElementById("<%=txt_par_cc3.ClientID %>");
            idval.style.display = "none";
            document.getElementById('<%=txt_par_cc3.ClientID %>').value = "";
            return false;
        }

        function parent_change4() {
            var idval = document.getElementById("<%=txt_par_studto1.ClientID %>");
            idval.style.display = "block";
            return false;
        }

        function parent_change44() {
            var idval = document.getElementById("<%=txt_par_studto1.ClientID %>");
            idval.style.display = "none";
            document.getElementById('<%=txt_par_studto1.ClientID %>').value = "";
            return false;
        }

        function parnt_checkchange1() {
            if (document.getElementById('<%=cb_par_dept.ClientID %>').checked == true) {
                var idval = document.getElementById("<%=div_parnt_dept.ClientID %>");
                idval.style.display = "block";
                return false;
            }
            else {
                var idval = document.getElementById("<%=div_parnt_dept.ClientID %>");
                idval.style.display = "none";
                return false;
            }
        }
        function parnt_checkchange2() {
            if (document.getElementById('<%=cb_par_indi.ClientID %>').checked == true) {
                var idval = document.getElementById("<%=div_parnt_indi.ClientID %>");
                idval.style.display = "block";
                return false;
            }
            else {
                var idval = document.getElementById("<%=div_parnt_indi.ClientID %>");
                idval.style.display = "none";
                return false;
            }
        }

        function parnt_checkchange3() {
            if (document.getElementById('<%=cb_par_stud.ClientID %>').checked == true) {
                var idval = document.getElementById("<%=div_par_stud.ClientID %>");
                idval.style.display = "block";
                return false;
            }
            else {
                var idval = document.getElementById("<%=div_par_stud.ClientID %>");
                idval.style.display = "none";
                return false;
            }
        }

        //        ************************* comPany ******************************


        function getcompany(txt1) {

            $.ajax({
                type: "POST",
                url: "LetterDocumentInward.aspx/comapanydetail",
                data: '{Name: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    bindmthdcompany(response.d);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }

        function getcompany1(txt1) {

            $.ajax({
                type: "POST",
                url: "LetterDocumentInward.aspx/comapanydetailperson",
                data: '{Name: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    bindmthdcompany(response.d);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }

        function bindmthdcompany(Employees) {
            var name = Employees[0].Name;
            document.getElementById('<%=txt_cname.ClientID %>').value = name;

            var pername = Employees[0].Per_Name;
            document.getElementById('<%=txt_pername.ClientID %>').value = pername;

            var des = Employees[0].Designation;
            document.getElementById('<%=txt_des.ClientID %>').value = des;

            var add = Employees[0].Address;
            document.getElementById('<%=txt_com_add.ClientID %>').value = add;


            var street = Employees[0].Street;
            document.getElementById('<%=txt_com_str.ClientID %>').value = street;

            var city = Employees[0].City;
            document.getElementById('<%=txt_com_city.ClientID %>').value = city;

            var country = Employees[0].Country;
            document.getElementById('<%=txt_com_county.ClientID %>').value = country;

            var pin = Employees[0].Pin;
            document.getElementById('<%=txt_com_pin.ClientID %>').value = pin;

            var state = Employees[0].State;
            document.getElementById('<%=txt_com_state.ClientID %>').value = state;

            var phn = Employees[0].Phone;
            document.getElementById('<%=txt_com_ph.ClientID %>').value = phn;

            var mail = Employees[0].MailId;
            document.getElementById('<%=txt_com_mail.ClientID %>').value = mail;

        }

        function changecompany1(id) {
            var value1 = id.value;
            if (value1.trim().toUpperCase() == "OTHERS") {
                var idval = document.getElementById("<%=txt_com_doc1.ClientID %>");
                idval.style.display = "block";
            }
            else {
                var idval = document.getElementById("<%=txt_com_doc1.ClientID %>");
                idval.style.display = "none";
            }
        }
        function changecompanyddl(id) {
            var value1 = id.value;
            if (value1.trim().toUpperCase() == "OTHERS") {
                var idval = document.getElementById("<%=txt_stud_doc_others.ClientID %>");
                idval.style.display = "block";
            }
            else {
                var idval = document.getElementById("<%=txt_stud_doc_others.ClientID %>");
                idval.style.display = "none";
            }
        }


        function changecompany2(id) {
            var value1 = id.value;
            if (value1.trim().toUpperCase() == "OTHERS") {
                var idval = document.getElementById("<%=txt_com_deliv1.ClientID %>");
                idval.style.display = "block";
            }
            else {
                var idval = document.getElementById("<%=txt_com_deliv1.ClientID %>");
                idval.style.display = "none";
            }
        }


        function changestud(id) {
            var value1 = id.value;
            if (value1.trim().toUpperCase() == "OTHERS") {
                var idval = document.getElementById("<%=txt_stud_delivery.ClientID %>");
                idval.style.display = "block";
            }
            else {
                var idval = document.getElementById("<%=txt_stud_delivery.ClientID %>");
                idval.style.display = "none";
            }
        }
        function comp_change() {
            var idval = document.getElementById("<%=txt_comdepto1.ClientID %>");
            idval.style.display = "block";
            return false;
        }
        function comp_change0() {
            var idval = document.getElementById("<%=txt_comdepto1.ClientID %>");
            idval.style.display = "none";
            document.getElementById('<%=txt_comdepto1.ClientID %>').value = "";
            return false;
        }

        function comp_change1() {
            var idval = document.getElementById("<%=txt_comcc2.ClientID %>");
            idval.style.display = "block";
            return false;
        }
        function comp_change11() {
            var idval = document.getElementById("<%=txt_comcc2.ClientID %>");
            idval.style.display = "none";
            document.getElementById('<%=txt_comcc2.ClientID %>').value = "";
            return false;
        }
        function comp_change2() {
            var idval = document.getElementById("<%=txt_com_ind1.ClientID %>");
            idval.style.display = "block";
            return false;
        }

        function comp_change22() {
            var idval = document.getElementById("<%=txt_com_ind1.ClientID %>");
            idval.style.display = "none";
            document.getElementById('<%=txt_com_ind1.ClientID %>').value = "";
            return false;
        }

        function comp_change3() {
            var idval = document.getElementById("<%=txt_com_cc1.ClientID %>");
            idval.style.display = "block";
            return false;
        }
        function comp_change33() {
            var idval = document.getElementById("<%=txt_com_cc1.ClientID %>");
            idval.style.display = "none";
            document.getElementById('<%=txt_com_cc1.ClientID %>').value = "";
            return false;
        }
        function comp_checkchange1() {
            if (document.getElementById('<%=cb_com_dept.ClientID %>').checked == true) {
                var idval = document.getElementById("<%=div_comp_dept.ClientID %>");
                idval.style.display = "block";
                return false;
            }
            else {
                var idval = document.getElementById("<%=div_comp_dept.ClientID %>");
                idval.style.display = "none";
                return false;
            }
        }
        function comp_checkchange2() {
            if (document.getElementById('<%=cb_com_indi.ClientID %>').checked == true) {
                var idval = document.getElementById("<%=div_comp_indi.ClientID %>");
                idval.style.display = "block";
                return false;
            }
            else {
                var idval = document.getElementById("<%=div_comp_indi.ClientID %>");
                idval.style.display = "none";
                return false;
            }
        }
        //************************************MAGAZINE***************************


        function getmagazines(txt1) {

            $.ajax({
                type: "POST",
                url: "LetterDocumentInward.aspx/magazinedetail",
                data: '{Name: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    bindmthdmaga(response.d);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }

        function bindmthdmaga(Employees) {

            var name = Employees[0].Name;
            document.getElementById('<%=txt_magzname.ClientID %>').value = name;

            var pername = Employees[0].Per_Name;
            document.getElementById('<%=txt_magztype.ClientID %>').value = pername;

            var des = Employees[0].Address;
            document.getElementById('<%=txt_magz_add.ClientID %>').value = des;


            var street = Employees[0].Street;
            document.getElementById('<%=txt_magz_str.ClientID %>').value = street;

            var city = Employees[0].City;
            document.getElementById('<%=txt_magz_city.ClientID %>').value = city;

            var pin = Employees[0].Pin;
            document.getElementById('<%=txt_pin.ClientID %>').value = pin;

            var country = Employees[0].Country;
            document.getElementById('<%=txt_magz_county.ClientID %>').value = country;

            var state = Employees[0].State;
            document.getElementById('<%=txt_magz_state.ClientID %>').value = state;

            var phn = Employees[0].Phone;
            document.getElementById('<%=txt_magz_ph.ClientID %>').value = phn;

            var mail = Employees[0].MailId;
            document.getElementById('<%=txt_magz_mail.ClientID %>').value = mail;


        }

        function getothers(txt1) {

            $.ajax({
                type: "POST",
                url: "LetterDocumentInward.aspx/othersdetail",
                data: '{Name: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    bindmthdothers(response.d);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }


        function getothers1(txt1) {

            $.ajax({
                type: "POST",
                url: "LetterDocumentInward.aspx/othersdetailname",
                data: '{Name: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    bindmthdothers(response.d);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function bindmthdothers(Employees) {

            var name = Employees[0].Name;
            document.getElementById('<%=txt_othr_name.ClientID %>').value = name;

            var pername = Employees[0].Per_Name;
            document.getElementById('<%=txt_othr_pname.ClientID %>').value = pername;

            var des = Employees[0].Address;
            document.getElementById('<%=txt_othr_add.ClientID %>').value = des;


            var street = Employees[0].Street;
            document.getElementById('<%=txt_othr_str.ClientID %>').value = street;

            var city = Employees[0].City;
            document.getElementById('<%=txt_othr_city.ClientID %>').value = city;

            var country = Employees[0].Country;
            document.getElementById('<%=txt_othr_county.ClientID %>').value = country;

            var pin = Employees[0].Pin;
            document.getElementById('<%=txt_othr_pin.ClientID %>').value = pin;

            var state = Employees[0].State;
            document.getElementById('<%=txt_othr_state.ClientID %>').value = state;

            var phn = Employees[0].Phone;
            document.getElementById('<%=txt_othr_ph.ClientID %>').value = phn;

            var mail = Employees[0].MailId;
            document.getElementById('<%=txt_othr_mail.ClientID %>').value = mail;


        }

        function changemag1(id) {
            var value1 = id.value;
            if (value1.trim().toUpperCase() == "OTHERS") {
                var idval = document.getElementById("<%=txt_magz_doc.ClientID %>");
                idval.style.display = "block";
            }
            else {
                var idval = document.getElementById("<%=txt_magz_doc.ClientID %>");
                idval.style.display = "none";
            }
        }

        function changemag2(id) {
            var value1 = id.value;
            if (value1.trim().toUpperCase() == "OTHERS") {
                var idval = document.getElementById("<%=txt_magz_deliv1.ClientID %>");
                idval.style.display = "block";
            }
            else {
                var idval = document.getElementById("<%=txt_magz_deliv1.ClientID %>");
                idval.style.display = "none";
            }
        }
        function magz_change() {
            var idval = document.getElementById("<%=txt_magz_depto1.ClientID %>");
            idval.style.display = "block";
            return false;
        }
        function magz_change0() {
            var idval = document.getElementById("<%=txt_magz_depto1.ClientID %>");
            idval.style.display = "none";
            document.getElementById('<%=txt_magz_depto1.ClientID %>').value = "";
            return false;
        }
        function magz_change1() {
            var idval = document.getElementById("<%=txt_magz_cc1.ClientID %>");
            idval.style.display = "block";
            return false;
        }

        function magz_change11() {
            var idval = document.getElementById("<%=txt_magz_cc1.ClientID %>");
            idval.style.display = "none";
            document.getElementById('<%=txt_magz_cc1.ClientID %>').value = "";
            return false;
        }
        function magz_change2() {
            var idval = document.getElementById("<%=txt_magz_indi1.ClientID %>");
            idval.style.display = "block";
            return false;
        }
        function magz_change22() {
            var idval = document.getElementById("<%=txt_magz_indi1.ClientID %>");
            idval.style.display = "none";
            document.getElementById('<%=txt_magz_indi1.ClientID %>').value = "";
            return false;
        }
        function magz_change3() {
            var idval = document.getElementById("<%=txt_magz_cc3.ClientID %>");
            idval.style.display = "block";
            return false;
        }
        function magz_change33() {
            var idval = document.getElementById("<%=txt_magz_cc3.ClientID %>");
            idval.style.display = "none";
            document.getElementById('<%=txt_magz_cc3.ClientID %>').value = "";
            return false;
        }
        function magz_checkchange1() {
            if (document.getElementById('<%=cb_magz_dept.ClientID %>').checked == true) {
                var idval = document.getElementById("<%=div_magz_dept.ClientID %>");
                idval.style.display = "block";
                return false;
            }
            else {
                var idval = document.getElementById("<%=div_magz_dept.ClientID %>");
                idval.style.display = "none";
                return false;
            }
        }
        function magz_checkchange2() {
            if (document.getElementById('<%=cb_magz_indi.ClientID %>').checked == true) {
                var idval = document.getElementById("<%=div_magz_indi.ClientID %>");
                idval.style.display = "block";
                return false;
            }
            else {
                var idval = document.getElementById("<%=div_magz_indi.ClientID %>");
                idval.style.display = "none";
                return false;
            }
        }

        //***********************individual******************************

        function getindividual(txt1) {

            $.ajax({
                type: "POST",
                url: "LetterDocumentInward.aspx/individualdetail",
                data: '{Name: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    bindmthdindividual(response.d);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }

        function bindmthdindividual(Employees) {

            var name = Employees[0].Name;
            document.getElementById('<%=txt_ind_cname.ClientID %>').value = name;

            var pername = Employees[0].Per_Name;
            document.getElementById('<%=txt_ind_pname.ClientID %>').value = pername;

            var add = Employees[0].Address;
            document.getElementById('<%=txt_ind_add.ClientID %>').value = add;

            var des = Employees[0].Designation;
            document.getElementById('<%=txt_ind_des.ClientID %>').value = des;

            var street = Employees[0].Street;
            document.getElementById('<%=txt_ind_str.ClientID %>').value = street;

            var city = Employees[0].City;
            document.getElementById('<%=txt_ind_city.ClientID %>').value = city;

            var pin = Employees[0].Pin;
            document.getElementById('<%=txt_ind_pin.ClientID %>').value = pin;

            var state = Employees[0].State;
            document.getElementById('<%=txt_ind_state.ClientID %>').value = state;

            var country = Employees[0].Country;
            document.getElementById('<%=txt_ind_county.ClientID %>').value = country;

            var phn = Employees[0].Phone;
            document.getElementById('<%=txt_ind_ph.ClientID %>').value = phn;

            var mail = Employees[0].MailId;
            document.getElementById('<%=txt_ind_mail.ClientID %>').value = mail;


        }


        function changeindi1(id) {
            var value1 = id.value;
            if (value1.trim().toUpperCase() == "OTHERS") {
                var idval = document.getElementById("<%=txt_ind_doc1.ClientID %>");
                idval.style.display = "block";
            }
            else {
                var idval = document.getElementById("<%=txt_ind_doc1.ClientID %>");
                idval.style.display = "none";
            }
        }

        function changeindi2(id) {
            var value1 = id.value;
            if (value1.trim().toUpperCase() == "OTHERS") {
                var idval = document.getElementById("<%=txt_ind_deliv1.ClientID %>");
                idval.style.display = "block";
            }
            else {
                var idval = document.getElementById("<%=txt_ind_deliv1.ClientID %>");
                idval.style.display = "none";
            }
        }

        function indiv_change() {
            var idval = document.getElementById("<%=txt_ind_depto1.ClientID %>");
            idval.style.display = "block";
            return false;
        }

        function indiv_change0() {
            var idval = document.getElementById("<%=txt_ind_depto1.ClientID %>");
            idval.style.display = "none";
            document.getElementById('<%=txt_ind_depto1.ClientID %>').value = "";
            return false;
        }

        function indiv_change1() {
            var idval = document.getElementById("<%=txt_ind_cc1.ClientID %>");
            idval.style.display = "block";
            return false;
        }
        function indiv_change11() {
            var idval = document.getElementById("<%=txt_ind_cc1.ClientID %>");
            idval.style.display = "none";
            document.getElementById('<%=txt_ind_cc1.ClientID %>').value = "";
            return false;
        }

        function indiv_change2() {
            var idval = document.getElementById("<%=txt_ind_indiv1.ClientID %>");
            idval.style.display = "block";
            return false;
        }
        function indiv_change22() {
            var idval = document.getElementById("<%=txt_ind_indiv1.ClientID %>");
            idval.style.display = "none";
            document.getElementById('<%=txt_ind_indiv1.ClientID %>').value = "";
            return false;
        }
        function indiv_change3() {
            var idval = document.getElementById("<%=txt_ind_indivcc1.ClientID %>");
            idval.style.display = "block";
            return false;
        }

        function indiv_change33() {
            var idval = document.getElementById("<%=txt_ind_indivcc1.ClientID %>");
            idval.style.display = "none";
            document.getElementById('<%=txt_ind_indivcc1.ClientID %>').value = "";
            return false;
        }
        function indiv_checkchange1() {
            if (document.getElementById('<%=cb_ind_dept.ClientID %>').checked == true) {
                var idval = document.getElementById("<%=div_indiv_dept.ClientID %>");
                idval.style.display = "block";
                return false;
            }
            else {
                var idval = document.getElementById("<%=div_indiv_dept.ClientID %>");
                idval.style.display = "none";
                return false;
            }
        }
        function indiv_checkchange2() {
            if (document.getElementById('<%=cb_ind_indi.ClientID %>').checked == true) {
                var idval = document.getElementById("<%=div_indiv_indi.ClientID %>");
                idval.style.display = "block";
                return false;
            }
            else {
                var idval = document.getElementById("<%=div_indiv_indi.ClientID %>");
                idval.style.display = "none";
                return false;
            }
        }
        //*********************** others ***************************

        function changeother1(id) {
            var value1 = id.value;
            if (value1.trim().toUpperCase() == "OTHERS") {
                var idval = document.getElementById("<%=txt_othr_doc.ClientID %>");
                idval.style.display = "block";
            }
            else {
                var idval = document.getElementById("<%=txt_othr_doc.ClientID %>");
                idval.style.display = "none";
            }
        }

        function changeother2(id) {
           
            var value1 = id.value;
            if (value1.trim().toUpperCase() == "OTHERS") {
                var idval = document.getElementById("<%=txt_othr_delivry.ClientID %>");
                idval.style.display = "block";
            }
            else {
                var idval = document.getElementById("<%=txt_othr_delivry.ClientID %>");
                idval.style.display = "none";
            }

        }

        function other_change() {
            var idval = document.getElementById("<%=txt_othr_depto1.ClientID %>");
            idval.style.display = "block";
            return false;
        }

        function other_change0() {
            var idval = document.getElementById("<%=txt_othr_depto1.ClientID %>");
            idval.style.display = "none";
            document.getElementById('<%=txt_othr_depto1.ClientID %>').value = "";
            return false;
        }

        function other_change1() {
            var idval = document.getElementById("<%=txt_othr_cc1.ClientID %>");
            idval.style.display = "block";
            return false;
        }

        function other_change11() {
            var idval = document.getElementById("<%=txt_othr_cc1.ClientID %>");
            idval.style.display = "none";
            document.getElementById('<%=txt_othr_cc1.ClientID %>').value = "";
            return false;
        }

        function other_change2() {
            var idval = document.getElementById("<%=txt_othr_indi1.ClientID %>");
            idval.style.display = "block";
            return false;
        }

        function other_change22() {
            var idval = document.getElementById("<%=txt_othr_indi1.ClientID %>");
            idval.style.display = "none";
            document.getElementById('<%=txt_othr_indi1.ClientID %>').value = "";
            return false;
        }

        function other_change3() {
            var idval = document.getElementById("<%=txt_othr_cc3.ClientID %>");
            idval.style.display = "block";
            return false;
        }

        function other_change33() {
            var idval = document.getElementById("<%=txt_othr_cc3.ClientID %>");
            idval.style.display = "none";
            document.getElementById('<%=txt_othr_cc3.ClientID %>').value = "";
            return false;
        }

        function other_checkchange1() {
            if (document.getElementById('<%=cb_othr_dept.ClientID %>').checked == true) {
                var idval = document.getElementById("<%=div_other_dept.ClientID %>");
                idval.style.display = "block";
                return false;
            }
            else {
                var idval = document.getElementById("<%=div_other_dept.ClientID %>");
                idval.style.display = "none";
                return false;
            }
        }
        function other_checkchange2() {
            if (document.getElementById('<%=cb_othr_indi.ClientID %>').checked == true) {
                var idval = document.getElementById("<%=div_other_indi.ClientID %>");
                idval.style.display = "block";
                return false;
            }
            else {
                var idval = document.getElementById("<%=div_other_indi.ClientID %>");
                idval.style.display = "none";
                return false;
            }
        }
        function studentcheck() {
            var stud = document.getElementById("<%=div_stud.ClientID %>");
            stud.style.display = "block";
            document.getElementById("<%=td_stud.ClientID %>").style.backgroundColor = '#c4c4c4';
            document.getElementById("<%=td_staff.ClientID %>").style.backgroundColor = 'white';
            document.getElementById("<%=td_par.ClientID %>").style.backgroundColor = 'white';
            document.getElementById("<%=td_comp.ClientID %>").style.backgroundColor = 'white';
            document.getElementById("<%=td_indi.ClientID %>").style.backgroundColor = 'white';
            document.getElementById("<%=td_mag.ClientID %>").style.backgroundColor = 'white';
            document.getElementById("<%=td_othr.ClientID %>").style.backgroundColor = 'white';

            var staff = document.getElementById("<%=div_staff.ClientID %>");
            staff.style.display = "none";

            var spread1 = document.getElementById("<%=spread.ClientID %>");
            spread1.style.display = "none";
            
//            delsi2402

            var parent = document.getElementById("<%=div_parent.ClientID %>");
            parent.style.display = "none";

            var company = document.getElementById("<%=div_company.ClientID %>");
            company.style.display = "none";

            var magaz = document.getElementById("<%=div_magazine.ClientID %>");
            magaz.style.display = "none";

            var other = document.getElementById("<%=div_others.ClientID %>");
            other.style.display = "none";

            var indiv = document.getElementById("<%=div_individu.ClientID %>");
            indiv.style.display = "none";

            var college = document.getElementById("<%=div_college.ClientID %>");
            college.style.display = "block";

            //rb_in.checked = true;
            document.getElementById("<%=rb_in.ClientID %>").checked = true;
            
            var ik = document.getElementById("cheiddiv");
            ik.style.cssFloat = "left";

            parent_div_clear();
            staff_div_clear(); 
            div_company_clear();
            div_individual_clear();
            div_mag_clear();
            div_others_clear();


            return false;

        }


        function staffcheck() {
            var stud = document.getElementById("<%=div_stud.ClientID %>");
            stud.style.display = "none";

            document.getElementById("<%=td_stud.ClientID %>").style.backgroundColor = 'white';
            document.getElementById("<%=td_staff.ClientID %>").style.backgroundColor = '#c4c4c4';
            document.getElementById("<%=td_par.ClientID %>").style.backgroundColor = 'white';
            document.getElementById("<%=td_comp.ClientID %>").style.backgroundColor = 'white';
            document.getElementById("<%=td_indi.ClientID %>").style.backgroundColor = 'white';
            document.getElementById("<%=td_mag.ClientID %>").style.backgroundColor = 'white';
            document.getElementById("<%=td_othr.ClientID %>").style.backgroundColor = 'white';


            var staff = document.getElementById("<%=div_staff.ClientID %>");
            staff.style.display = "block";

            var staffdiv = document.getElementById("<%=div2.ClientID %>");
            staffdiv.style.display = "block";

            var parent = document.getElementById("<%=div_parent.ClientID %>");
            parent.style.display = "none";

            var company = document.getElementById("<%=div_company.ClientID %>");
            company.style.display = "none";

            var spread1 = document.getElementById("<%=spread.ClientID %>");
            spread1.style.display = "none";

            var magaz = document.getElementById("<%=div_magazine.ClientID %>");
            magaz.style.display = "none";

            var other = document.getElementById("<%=div_others.ClientID %>");
            other.style.display = "none";

            var indiv = document.getElementById("<%=div_individu.ClientID %>");
            indiv.style.display = "none";
            var college = document.getElementById("<%=div_college.ClientID %>");
            college.style.display = "block";
            document.getElementById("<%=rb_stf_in.ClientID %>").checked = true;
            //rb_stf_in.checked = true;
           
            var ik = document.getElementById("staffchngdiv");
            ik.style.cssFloat = "left";

            parent_div_clear();
            div_company_clear();
            div_individual_clear();
            div_mag_clear();
            div_others_clear();
            div_stud_clear(); 

            return false;

        }

        function parentcheck() {
            document.getElementById("<%=td_stud.ClientID %>").style.backgroundColor = 'white';
            document.getElementById("<%=td_staff.ClientID %>").style.backgroundColor = 'white';
            document.getElementById("<%=td_par.ClientID %>").style.backgroundColor = '#c4c4c4';
            document.getElementById("<%=td_comp.ClientID %>").style.backgroundColor = 'white';
            document.getElementById("<%=td_indi.ClientID %>").style.backgroundColor = 'white';
            document.getElementById("<%=td_mag.ClientID %>").style.backgroundColor = 'white';
            document.getElementById("<%=td_othr.ClientID %>").style.backgroundColor = 'white';

            var stud = document.getElementById("<%=div_stud.ClientID %>");
            stud.style.display = "none";

            var staff = document.getElementById("<%=div_staff.ClientID %>");
            staff.style.display = "none";

            var parent = document.getElementById("<%=div_parent.ClientID %>");
            parent.style.display = "block";

            var parentdiv = document.getElementById("<%=div3.ClientID %>");
            parentdiv.style.display = "block";

            var company = document.getElementById("<%=div_company.ClientID %>");
            company.style.display = "none";

            var magaz = document.getElementById("<%=div_magazine.ClientID %>");
            magaz.style.display = "none";

            var other = document.getElementById("<%=div_others.ClientID %>");
            other.style.display = "none";

            var indiv = document.getElementById("<%=div_individu.ClientID %>");
            indiv.style.display = "none";
            //rb_par_in.checked = true;
            document.getElementById('<%=rb_par_in.ClientID %>').checked = true;
            var college = document.getElementById("<%=div_college.ClientID %>");
            college.style.display = "block";

            var ik = document.getElementById("parchngdiv");
            ik.style.cssFloat = "left";


            staff_div_clear();
            div_company_clear();
            div_individual_clear();
            div_mag_clear();
            div_others_clear();
            div_stud_clear();

            return false;

        }

        function companycheck() {
            document.getElementById("<%=td_stud.ClientID %>").style.backgroundColor = 'white';
            document.getElementById("<%=td_staff.ClientID %>").style.backgroundColor = 'white';
            document.getElementById("<%=td_par.ClientID %>").style.backgroundColor = 'white';
            document.getElementById("<%=td_comp.ClientID %>").style.backgroundColor = '#c4c4c4';
            document.getElementById("<%=td_indi.ClientID %>").style.backgroundColor = 'white';
            document.getElementById("<%=td_mag.ClientID %>").style.backgroundColor = 'white';
            document.getElementById("<%=td_othr.ClientID %>").style.backgroundColor = 'white';


            var stud = document.getElementById("<%=div_stud.ClientID %>");
            stud.style.display = "none";

            var spread1 = document.getElementById("<%=spread.ClientID %>");
            spread1.style.display = "none";
            var staff = document.getElementById("<%=div_staff.ClientID %>");
            staff.style.display = "none";

            var parent = document.getElementById("<%=div_parent.ClientID %>");
            parent.style.display = "none";

            var company = document.getElementById("<%=div_company.ClientID %>");
            company.style.display = "block";

            var companydiv = document.getElementById("<%=div7.ClientID %>");
            companydiv.style.display = "block";

            var magaz = document.getElementById("<%=div_magazine.ClientID %>");
            magaz.style.display = "none";

            var other = document.getElementById("<%=div_others.ClientID %>");
            other.style.display = "none";

            var indiv = document.getElementById("<%=div_individu.ClientID %>");
            indiv.style.display = "none";
            var college = document.getElementById("<%=div_college.ClientID %>");
            college.style.display = "block";
            document.getElementById('<%=rb_comp_in.ClientID %>').checked = true;
            //rb_comp_in.checked = true;

            var ik = document.getElementById("compchngdiv");
            ik.style.cssFloat = "left";

            parent_div_clear();
            staff_div_clear();

            div_individual_clear();
            div_mag_clear();
            div_others_clear();
            div_stud_clear();

            return false;

        }

        function magazinecheck() {
            document.getElementById("<%=td_stud.ClientID %>").style.backgroundColor = 'white';
            document.getElementById("<%=td_staff.ClientID %>").style.backgroundColor = 'white';
            document.getElementById("<%=td_par.ClientID %>").style.backgroundColor = 'white';
            document.getElementById("<%=td_comp.ClientID %>").style.backgroundColor = 'white';
            document.getElementById("<%=td_indi.ClientID %>").style.backgroundColor = 'white';
            document.getElementById("<%=td_mag.ClientID %>").style.backgroundColor = '#c4c4c4';
            document.getElementById("<%=td_othr.ClientID %>").style.backgroundColor = 'white';

            var stud = document.getElementById("<%=div_stud.ClientID %>");
            stud.style.display = "none";

            var staff = document.getElementById("<%=div_staff.ClientID %>");
            staff.style.display = "none";

            var parent = document.getElementById("<%=div_parent.ClientID %>");
            parent.style.display = "none";

            var company = document.getElementById("<%=div_company.ClientID %>");
            company.style.display = "none";

            var magaz = document.getElementById("<%=div_magazine.ClientID %>");
            magaz.style.display = "block";

            var magdiv = document.getElementById("<%=div4.ClientID %>");
            magdiv.style.display = "block";

            var other = document.getElementById("<%=div_others.ClientID %>");
            other.style.display = "none";

            var indiv = document.getElementById("<%=div_individu.ClientID %>");
            indiv.style.display = "none";

            var college = document.getElementById("<%=div_college.ClientID %>");
            college.style.display = "block";
            //rb_magz_in.checked = true;
            document.getElementById("<%=rb_magz_in.ClientID %>").checked = true;

            var ik = document.getElementById("magchngdiv");
            ik.style.cssFloat = "left";

            parent_div_clear();
            staff_div_clear();
            div_company_clear();
            div_individual_clear();

            div_others_clear();
            div_stud_clear();

            return false;

        }
        function otherscheck() {
            document.getElementById("<%=td_stud.ClientID %>").style.backgroundColor = 'white';
            document.getElementById("<%=td_staff.ClientID %>").style.backgroundColor = 'white';
            document.getElementById("<%=td_par.ClientID %>").style.backgroundColor = 'white';
            document.getElementById("<%=td_comp.ClientID %>").style.backgroundColor = 'white';
            document.getElementById("<%=td_indi.ClientID %>").style.backgroundColor = 'white';
            document.getElementById("<%=td_mag.ClientID %>").style.backgroundColor = 'white';
            document.getElementById("<%=td_othr.ClientID %>").style.backgroundColor = '#c4c4c4'; 
         
            var stud = document.getElementById("<%=div_stud.ClientID %>");
            stud.style.display = "none";

            var spread1 = document.getElementById("<%=spread.ClientID %>");
            spread1.style.display = "none";
            
            var staff = document.getElementById("<%=div_staff.ClientID %>");
            staff.style.display = "none";

            var parent = document.getElementById("<%=div_parent.ClientID %>");
            parent.style.display = "none";

            var company = document.getElementById("<%=div_company.ClientID %>");
            company.style.display = "none";

            var magaz = document.getElementById("<%=div_magazine.ClientID %>");
            magaz.style.display = "none";

            var other = document.getElementById("<%=div_others.ClientID %>");
            other.style.display = "block";

            var otherdiv = document.getElementById("<%=div5.ClientID %>");
            otherdiv.style.display = "block";

            var indiv = document.getElementById("<%=div_individu.ClientID %>");
            indiv.style.display = "none";
            var college = document.getElementById("<%=div_college.ClientID %>");
            college.style.display = "block";
            //rb_othr_in.checked = true;
            document.getElementById("<%=rb_othr_in.ClientID %>").checked = true;
            var ik = document.getElementById("othrchngdiv");
            ik.style.cssFloat = "left";

            parent_div_clear();
            staff_div_clear();
            div_company_clear();

            div_mag_clear();
            div_individual_clear();
            div_stud_clear();
            

            return false;

        }
        function individualcheck() {
            document.getElementById("<%=td_stud.ClientID %>").style.backgroundColor = 'white';
            document.getElementById("<%=td_staff.ClientID %>").style.backgroundColor = 'white';
            document.getElementById("<%=td_par.ClientID %>").style.backgroundColor = 'white';
            document.getElementById("<%=td_comp.ClientID %>").style.backgroundColor = 'white';
            document.getElementById("<%=td_indi.ClientID %>").style.backgroundColor = '#c4c4c4';
            document.getElementById("<%=td_mag.ClientID %>").style.backgroundColor = 'white';
            document.getElementById("<%=td_othr.ClientID %>").style.backgroundColor = 'white';

            var indiv = document.getElementById("<%=div_individu.ClientID %>");
            indiv.style.display = "block";

            var stud = document.getElementById("<%=div_stud.ClientID %>");
            stud.style.display = "none";

            var staff = document.getElementById("<%=div_staff.ClientID %>");
            staff.style.display = "none";

            var parent = document.getElementById("<%=div_parent.ClientID %>");
            parent.style.display = "none";

            var company = document.getElementById("<%=div_company.ClientID %>");
            company.style.display = "none";

            var magaz = document.getElementById("<%=div_magazine.ClientID %>");
            magaz.style.display = "none";

            var other = document.getElementById("<%=div_others.ClientID %>");
            other.style.display = "none";

            var college = document.getElementById("<%=div_college.ClientID %>");
            college.style.display = "block";


            var indidiv = document.getElementById("<%=div6.ClientID %>");
            indidiv.style.display = "block";
            document.getElementById('<%=rb_indiv_in.ClientID %>').checked = true;
            //rb_indiv_in.checked = true;
            var ik = document.getElementById("indichngdiv");
            ik.style.cssFloat = "left";

            parent_div_clear();
            staff_div_clear();
            div_company_clear();

            div_mag_clear();
            div_others_clear();
            div_stud_clear();

            return false;

        }


        //***************** roll_no******************
        function checkrno(txt1) {
            $.ajax({
                type: "POST",
                url: "LetterDocumentInward.aspx/CheckRollNo",
                data: '{Roll_No: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess,
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function OnSuccess(response) {
            var mesg = $("#rnomsg")[0];

            switch (response.d) {
                case "0":

                    mesg.style.color = "red";
                    mesg.innerHTML = "RollNo not exist";
                    studentclear();
                    break;
                case "1":
                    get();
                    mesg.style.color = "green";
                    mesg.innerHTML = "Available";
                    break;
                case "error":
                    mesg.style.color = "red";
                    mesg.innerHTML = "Error occurred";
                    break;
            }
        }
        /////////////regno
        function checkregno(txt1) {
            $.ajax({
                type: "POST",
                url: "LetterDocumentInward.aspx/CheckRegNodet",
                data: '{Roll_No: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccessRegno,
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function OnSuccessRegno(response) {
            var mesg = $("#regmsg")[0];  
            switch (response.d) {
                case "0":

                    mesg.style.color = "red";
                    mesg.innerHTML = "RegNo not exist";
                    studentclear();
                    break;
                case "1":
                    getreg();
                    mesg.style.color = "green";
                    mesg.innerHTML = "Available";
                    break;
                case "error":
                    mesg.style.color = "red";
                    mesg.innerHTML = "Error occurred";
                    break;
            }
        }

        function checkdepartment(txt1) {
            $.ajax({
                type: "POST",
                url: "LetterDocumentInward.aspx/Checkdept",
                data: '{Roll_No: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccessdept,
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function OnSuccessdept(response) {
            var mesg = $("#deptmsg")[0];
            var mesg1 = $("#deptmsg1")[0];
            var mesg2 = $("#deptmsg2")[0];
            var mesg3 = $("#deptmsg3")[0];
            var mesg4 = $("#deptmsg4")[0];
            var mesg5 = $("#deptmsg5")[0];
            var mesg6 = $("#deptmsg6")[0];
            switch (response.d) {
                case "0":

                    mesg.style.color = "red";
                    mesg.innerHTML = "Dept not exist";

                    break;
                case "1":
                    get();
                    mesg.style.color = "green";
                    mesg.innerHTML = "Dept Available";

                    break;
                case "2":
                    mesg.innerHTML = "";
                    break;
                case "error":
                    mesg.style.color = "red";
                    mesg.innerHTML = "Error occurred";
                    break;
            }
        }

        function checkdepartment1(txt1) {
            $.ajax({
                type: "POST",
                url: "LetterDocumentInward.aspx/Checkdept",
                data: '{Roll_No: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccessdept1,
                failure: function (response) {
                    alert(response);
                }
            });
        }

        function OnSuccessdept1(response) {

            var mesg1 = $("#deptmsg1")[0];

            switch (response.d) {
                case "0":

                    mesg1.style.color = "red";
                    mesg1.innerHTML = "Dept not exist";
                    break;
                case "1":
                    get();
                    mesg1.style.color = "green";
                    mesg1.innerHTML = "Dept Available";

                    break;

                case "2":
                    mesg1.innerHTML = "";
                    break;

                case "error":
                    mesg.style.color = "red";
                    mesg.innerHTML = "Error occurred";
                    break;
            }
        }

        function checkdepartment2(txt1) {
            $.ajax({
                type: "POST",
                url: "LetterDocumentInward.aspx/Checkdept",
                data: '{Roll_No: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccessdept2,
                failure: function (response) {
                    alert(response);
                }
            });
        }

        function OnSuccessdept2(response) {

            var mesg2 = $("#deptmsg2")[0];

            switch (response.d) {
                case "0":

                    mesg2.style.color = "red";
                    mesg2.innerHTML = "Dept not exist";
                    break;
                case "1":
                    get();
                    mesg2.style.color = "green";
                    mesg2.innerHTML = "Dept Available";

                    break;
                case "2":
                    mesg2.innerHTML = "";
                    break;
                case "error":
                    mesg2.style.color = "red";
                    mesg2.innerHTML = "Error occurred";
                    break;
            }
        }

        function checkdepartment3(txt1) {
            $.ajax({
                type: "POST",
                url: "LetterDocumentInward.aspx/Checkdept",
                data: '{Roll_No: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccessdept3,
                failure: function (response) {
                    alert(response);
                }
            });
        }

        function OnSuccessdept3(response) {

            var mesg2 = $("#deptmsg3")[0];

            switch (response.d) {
                case "0":

                    mesg2.style.color = "red";
                    mesg2.innerHTML = "Dept not exist";
                    break;
                case "1":
                    get();
                    mesg2.style.color = "green";
                    mesg2.innerHTML = "Dept Available";

                    break;
                case "2":
                    mesg2.innerHTML = "";
                    break;
                case "error":
                    mesg2.style.color = "red";
                    mesg2.innerHTML = "Error occurred";
                    break;
            }
        }
        function checkdepartment4(txt1) {
            $.ajax({
                type: "POST",
                url: "LetterDocumentInward.aspx/Checkdept",
                data: '{Roll_No: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccessdept4,
                failure: function (response) {
                    alert(response);
                }
            });
        }

        function OnSuccessdept4(response) {

            var mesg2 = $("#deptmsg4")[0];

            switch (response.d) {
                case "0":

                    mesg2.style.color = "red";
                    mesg2.innerHTML = "Dept not exist";
                    break;
                case "1":
                    get();
                    mesg2.style.color = "green";
                    mesg2.innerHTML = "Dept Available";

                    break;
                case "2":
                    mesg2.innerHTML = "";
                    break;
                case "error":
                    mesg2.style.color = "red";
                    mesg2.innerHTML = "Error occurred";
                    break;
            }
        }

        function checkdepartment5(txt1) {
            $.ajax({
                type: "POST",
                url: "LetterDocumentInward.aspx/Checkdept",
                data: '{Roll_No: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccessdept5,
                failure: function (response) {
                    alert(response);
                }
            });
        }

        function OnSuccessdept5(response) {

            var mesg2 = $("#deptmsg5")[0];

            switch (response.d) {
                case "0":

                    mesg2.style.color = "red";
                    mesg2.innerHTML = "Dept not exist";
                    break;
                case "1":
                    get();
                    mesg2.style.color = "green";
                    mesg2.innerHTML = "Dept Available";

                    break;
                case "2":
                    mesg2.innerHTML = "";
                    break;
                case "error":
                    mesg2.style.color = "red";
                    mesg2.innerHTML = "Error occurred";
                    break;
            }
        }
        function checkdepartment6(txt1) {
            $.ajax({
                type: "POST",
                url: "LetterDocumentInward.aspx/Checkdept",
                data: '{Roll_No: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccessdept6,
                failure: function (response) {
                    alert(response);
                }
            });
        }

        function OnSuccessdept6(response) {

            var mesg2 = $("#deptmsg6")[0];

            switch (response.d) {
                case "0":

                    mesg2.style.color = "red";
                    mesg2.innerHTML = "Dept not exist";
                    break;
                case "1":
                    get();
                    mesg2.style.color = "green";
                    mesg2.innerHTML = "Dept Available";

                    break;
                case "2":
                    mesg2.innerHTML = "";
                    break;
                case "error":
                    mesg2.style.color = "red";
                    mesg2.innerHTML = "Error occurred";
                    break;
            }
        }

        function checkindiv(txt1) {
            $.ajax({
                type: "POST",
                url: "LetterDocumentInward.aspx/Checkindividual",
                data: '{Roll_No: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccessindiv,
                failure: function (response) {
                    alert(response);
                }
            });
        }

        function OnSuccessindiv(response) {

            var mesg2 = $("#indimsg")[0];

            switch (response.d) {
                case "0":

                    mesg2.style.color = "red";
                    mesg2.innerHTML = "Staff not exist";
                    break;
                case "1":
                    get();
                    mesg2.style.color = "green";
                    mesg2.innerHTML = "Staff Available";

                    break;
                case "2":
                    mesg2.innerHTML = "";
                    break;
                case "error":
                    mesg2.style.color = "red";
                    mesg2.innerHTML = "Error occurred";
                    break;
            }
        }

        function checkindiv1(txt1) {
            $.ajax({
                type: "POST",
                url: "LetterDocumentInward.aspx/Checkindividual",
                data: '{Roll_No: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccessindiv1,
                failure: function (response) {
                    alert(response);
                }
            });
        }

        function OnSuccessindiv1(response) {

            var mesg2 = $("#indimsg1")[0];

            switch (response.d) {
                case "0":

                    mesg2.style.color = "red";
                    mesg2.innerHTML = "Staff not exist";
                    break;
                case "1":
                    get();
                    mesg2.style.color = "green";
                    mesg2.innerHTML = "Staff Available";

                    break;
                case "2":
                    mesg2.innerHTML = "";
                    break;
                case "error":
                    mesg2.style.color = "red";
                    mesg2.innerHTML = "Error occurred";
                    break;
            }
        }

        function checkindiv2(txt1) {
            $.ajax({
                type: "POST",
                url: "LetterDocumentInward.aspx/Checkindividual",
                data: '{Roll_No: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccessindiv2,
                failure: function (response) {
                    alert(response);
                }
            });
        }

        function OnSuccessindiv2(response) {

            var mesg2 = $("#indimsg2")[0];

            switch (response.d) {
                case "0":

                    mesg2.style.color = "red";
                    mesg2.innerHTML = "Staff not exist";
                    break;
                case "1":
                    get();
                    mesg2.style.color = "green";
                    mesg2.innerHTML = "Available";

                    break;
                case "2":
                    mesg2.innerHTML = "";
                    break;
                case "error":
                    mesg2.style.color = "red";
                    mesg2.innerHTML = "Error occurred";
                    break;
            }
        }
        function checkindiv3(txt1) {
            $.ajax({
                type: "POST",
                url: "LetterDocumentInward.aspx/Checkindividual",
                data: '{Roll_No: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccessindiv3,
                failure: function (response) {
                    alert(response);
                }
            });
        }

        function OnSuccessindiv3(response) {

            var mesg2 = $("#indimsg3")[0];

            switch (response.d) {
                case "0":

                    mesg2.style.color = "red";
                    mesg2.innerHTML = "Staff not exist";
                    break;
                case "1":
                    get();
                    mesg2.style.color = "green";
                    mesg2.innerHTML = "Staff Available";

                    break;
                case "2":
                    mesg2.innerHTML = "";
                    break;
                case "error":
                    mesg2.style.color = "red";
                    mesg2.innerHTML = "Error occurred";
                    break;
            }
        }
        function checkindiv4(txt1) {
            $.ajax({
                type: "POST",
                url: "LetterDocumentInward.aspx/Checkindividual",
                data: '{Roll_No: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccessindiv4,
                failure: function (response) {
                    alert(response);
                }
            });
        }

        function OnSuccessindiv4(response) {

            var mesg2 = $("#indimsg4")[0];

            switch (response.d) {
                case "0":

                    mesg2.style.color = "red";
                    mesg2.innerHTML = "Staff not exist";
                    break;
                case "1":
                    get();
                    mesg2.style.color = "green";
                    mesg2.innerHTML = "Staff Available";

                    break;
                case "2":
                    mesg2.innerHTML = "";
                    break;
                case "error":
                    mesg2.style.color = "red";
                    mesg2.innerHTML = "Error occurred";
                    break;
            }
        }
        function checkindiv5(txt1) {
            $.ajax({
                type: "POST",
                url: "LetterDocumentInward.aspx/Checkindividual",
                data: '{Roll_No: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccessindiv5,
                failure: function (response) {
                    alert(response);
                }
            });
        }

        function OnSuccessindiv5(response) {

            var mesg2 = $("#indimsg5")[0];

            switch (response.d) {
                case "0":

                    mesg2.style.color = "red";
                    mesg2.innerHTML = "Staff not exist";
                    break;
                case "1":
                    get();
                    mesg2.style.color = "green";
                    mesg2.innerHTML = "Staff Available";

                    break;
                case "2":
                    mesg2.innerHTML = "";
                    break;
                case "error":
                    mesg2.style.color = "red";
                    mesg2.innerHTML = "Error occurred";
                    break;
            }
        }
        function checkindiv6(txt1) {
            $.ajax({
                type: "POST",
                url: "LetterDocumentInward.aspx/Checkindividual",
                data: '{Roll_No: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccessindiv6,
                failure: function (response) {
                    alert(response);
                }
            });
        }

        function OnSuccessindiv6(response) {

            var mesg2 = $("#indimsg6")[0];

            switch (response.d) {
                case "0":

                    mesg2.style.color = "red";
                    mesg2.innerHTML = "Staff not exist";
                    break;
                case "1":
                    get();
                    mesg2.style.color = "green";
                    mesg2.innerHTML = "Staff Available";

                    break;
                case "2":
                    mesg2.innerHTML = "";
                    break;
                case "error":
                    mesg2.style.color = "red";
                    mesg2.innerHTML = "Error occurred";
                    break;
            }
        }
        //        function checknamecomp(txt1) {
        //            $.ajax({
        //                type: "POST",
        //                url: "LetterDocumentInward.aspx/Checkcompany",
        //                data: '{Name: "' + txt1 + '"}',
        //                contentType: "application/json; charset=utf-8",
        //                dataType: "json",
        //                success: OnSuccess11,
        //                failure: function (response) {
        //                    alert(response);
        //                }
        //            });

        //           
        //        }
        //        function OnSuccess11(response) {
        //            var mesg = $("#rnomsgcomp")[0];

        //            switch (response.d) {
        //                case "0":                                  
        //                    div_company_clear();
        //                    break;
        //                case "1":
        //                    getcompany1();                
        //                    break;
        //                case "error":                  
        //                    break;
        //            }


        //        }



        function checkrno1(txt1) {
            $.ajax({
                type: "POST",
                url: "LetterDocumentInward.aspx/CheckRollNo1",
                data: '{Roll_No: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess1,
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function OnSuccess1(response) {

            var msg = $("#errmsg")[0];
            switch (response.d) {
                case "0":


                    msg.style.color = "red";
                    msg.innerHTML = "RollNo not exist";
                    studentclear();

                    break;
                case "1":

                    get();

                    msg.style.color = "green";
                    msg.innerHTML = "Available";
                    break;
                case "error":
                    msg.style.color = "red";
                    msg.innerHTML = "Error occurred";
                    break;
            }
        }
        function studentclear() {
            document.getElementById('<%=txt_name.ClientID %>').value = "";
            document.getElementById('<%=txt_batch.ClientID %>').value = "";
            document.getElementById('<%=txt_degree.ClientID %>').value = "";
            document.getElementById('<%=txt_department.ClientID %>').value = "";
            document.getElementById('<%=txt_sem.ClientID %>').value = "";
            document.getElementById('<%=txt_addrs.ClientID %>').value = "";
            document.getElementById('<%=txt_street.ClientID %>').value = "";
            document.getElementById('<%=txt_city.ClientID %>').value = "";
            document.getElementById('<%=txt_state.ClientID %>').value = "";
            document.getElementById('<%=txt_pho.ClientID %>').value = "";
            document.getElementById('<%=txt_mailid.ClientID %>').value = "";
            document.getElementById('<%=txt_studtype.ClientID %>').value = "";
            document.getElementById('<%=txt_section.ClientID %>').value = "";
            document.getElementById('<%=txt_pin_stud.ClientID %>').value = "";
            return false;
        }

        //****************  student div clear ****************

        function ddl_clear_stud() {
            var id = document.getElementById('<%= ddl_docpriv.ClientID %>');
            document.getElementById('<%= ddl_docpriv.ClientID %>').selectedIndex = "0";
            document.getElementById('<%= ddl_delivery.ClientID %>').selectedIndex = "0";
            //  alert(id.options[id.selectedIndex]);                     
        }

        function ddl_clear_staff() {
            document.getElementById('<%= ddl_stf_docpri.ClientID %>').selectedIndex = "0";
            document.getElementById('<%= ddl_stf_delivry.ClientID %>').selectedIndex = "0";
        }

        function ddl_clear_par() {
            document.getElementById('<%= ddl_par_doc.ClientID %>').selectedIndex = "0";
            document.getElementById('<%= ddl_par_delivry.ClientID %>').selectedIndex = "0";
        }

        function ddl_clear_com() {
            document.getElementById('<%= ddl_com_doc.ClientID %>').selectedIndex = "0";
            document.getElementById('<%= ddl_com_delivry.ClientID %>').selectedIndex = "0";
        }

        function ddl_clear_mag() {
            document.getElementById('<%= ddl_magz_doc.ClientID %>').selectedIndex = "0";
            document.getElementById('<%= ddl_magz_delivry.ClientID %>').selectedIndex = "0";
        }

        function ddl_clear_indi() {
            document.getElementById('<%= ddl_ind_doc.ClientID %>').selectedIndex = "0";
            document.getElementById('<%= ddl_ind_delivry.ClientID %>').selectedIndex = "0";
        }
        function ddl_clear_othr() {
            document.getElementById('<%= ddl_othr_doc.ClientID %>').selectedIndex = "0";
            document.getElementById('<%= ddl_othr_delivry.ClientID %>').selectedIndex = "0";
        }

        function div_stud_clear() {

            ddl_clear_stud();
            document.getElementById('<%=txt_roll.ClientID %>').value = "";
            document.getElementById('<%=txt_name.ClientID %>').value = "";
            document.getElementById('<%=txt_batch.ClientID %>').value = "";
            document.getElementById('<%=txt_degree.ClientID %>').value = "";
            document.getElementById('<%=txt_department.ClientID %>').value = "";
            document.getElementById('<%=txt_section.ClientID %>').value = "";
            document.getElementById('<%=txt_sem.ClientID %>').value = "";
            document.getElementById('<%=txt_stud_country.ClientID %>').value = "";
            document.getElementById('<%=txt_addrs.ClientID %>').value = "";
            document.getElementById('<%=txt_street.ClientID %>').value = "";
            document.getElementById('<%=txt_city.ClientID %>').value = "";
            document.getElementById('<%=txt_pin_stud.ClientID %>').value = "";
            document.getElementById('<%=txt_state.ClientID %>').value = "";
            document.getElementById('<%=txt_pho.ClientID %>').value = "";
            document.getElementById('<%=txt_mailid.ClientID %>').value = "";
            document.getElementById('<%=txt_studtype.ClientID %>').value = "";
            document.getElementById('<%=txt_dept_to.ClientID %>').value = "";
            document.getElementById('<%=txt_to1.ClientID %>').value = "";
            document.getElementById('<%=txt_dept_cc.ClientID %>').value = "";
            document.getElementById('<%=txt_cc1.ClientID %>').value = "";
            document.getElementById('<%=txt_indiv.ClientID %>').value = "";
            document.getElementById('<%=txt_indiv1.ClientID %>').value = "";
            document.getElementById('<%=txt_indiv_cc.ClientID %>').value = "";
            document.getElementById('<%=txt_cc2.ClientID %>').value = "";
            document.getElementById('<%=txt_subject.ClientID %>').value = "";
            document.getElementById('<%=txt_docpriv.ClientID %>').value = "";
            document.getElementById('<%=txt_delivery.ClientID %>').value = "";
            document.getElementById('<%=txt_bank.ClientID %>').value = "";
            document.getElementById('<%=ttx_refno.ClientID %>').value = "";
            document.getElementById('<%=fileupload.ClientID %>').value = "";
            document.getElementById("rnomsg").innerHTML = "";
            document.getElementById('<%=txt_chargestud.ClientID %>').value = "";
            document.getElementById('<%=txt_chequename.ClientID %>').value = "";
            document.getElementById('<%=txt_chequenumb.ClientID %>').value = "";
            document.getElementById('<%=txt_chequeamt.ClientID %>').value = "";
            document.getElementById('<%=txt_ddname.ClientID %>').value = "";
            document.getElementById('<%=txt_ddno.ClientID %>').value = "";
            document.getElementById('<%=txt_ddamt.ClientID %>').value = "";
            document.getElementById('<%=cb_paymentstud.ClientID %>').checked = false;
            document.getElementById('<%=txt_reg.ClientID %>').value = "";
            //document.getElementById('<%=txt_batch.ClientID %>').value = "";
           // document.getElementById('<%=txt_department.ClientID %>').value = "";
            //cb_paymentstud.checked = false;
            return false;
        }
        //***************** In Out change ************************


        function outchange() {
            var ik = document.getElementById("cheiddiv");
            ik.style.cssFloat = "right";

            return false;
        }

        function inchange() {
            var ik = document.getElementById("cheiddiv");
            ik.style.cssFloat = "left";

            return false;
        }


        function instaff() {
            var ik = document.getElementById("staffchngdiv");
            ik.style.cssFloat = "left";
            return false;
        }


        function outstaff() {

            var ik = document.getElementById("staffchngdiv");
            ik.style.cssFloat = "right";
            return false;
        }

        function inparent() {
            var ik = document.getElementById("parchngdiv");
            ik.style.cssFloat = "left";
            return false;
        }

        function outparent() {
            var ik = document.getElementById("parchngdiv");
            ik.style.cssFloat = "right";
            return false;
        }


        function incomp() {
            var ik = document.getElementById("compchngdiv");
            ik.style.cssFloat = "left";
            return false;
        }

        function outcomp() {
            var ik = document.getElementById("compchngdiv");
            ik.style.cssFloat = "right";
            return false;
        }


        function inindi() {
            var ik = document.getElementById("indichngdiv");
            ik.style.cssFloat = "left";
            return false;
        }

        function outindi() {
            var ik = document.getElementById("indichngdiv");
            ik.style.cssFloat = "right";
            return false;
        }

        function inmag() {
            var ik = document.getElementById("magchngdiv");
            ik.style.cssFloat = "left";
            return false;
        }


        function outmag() {
            var ik = document.getElementById("magchngdiv");
            ik.style.cssFloat = "right";
            return false;
        }


        function inothr() {
            var ik = document.getElementById("othrchngdiv");
            ik.style.cssFloat = "left";
            return false;
        }


        function outothr() {
            var ik = document.getElementById("othrchngdiv");
            ik.style.cssFloat = "right";
            return false;
        }




        //************************* staff div clear *********************


        function staff_div_clear() {
            ddl_clear_staff();
            document.getElementById('<%=txt_name1.ClientID %>').value = "";
            document.getElementById('<%=txt_stf_state.ClientID %>').value = "";

            document.getElementById('<%=txt_stf_code.ClientID %>').value = "";
            document.getElementById('<%=txt_stf_country.ClientID %>').value = "";
            document.getElementById('<%=txt_stf_ph.ClientID %>').value = "";
            document.getElementById('<%=txt_stf_mail.ClientID %>').value = "";
            document.getElementById('<%=txt_stf_depto.ClientID %>').value = "";
            document.getElementById('<%=txt_stf_depto1.ClientID %>').value = "";
            document.getElementById('<%=txt_stf_cc.ClientID %>').value = "";
            document.getElementById('<%=txt_stf_cc1.ClientID %>').value = "";
            document.getElementById('<%=txt_stf_indito.ClientID %>').value = "";
            document.getElementById('<%=txt_stf_indito1.ClientID %>').value = "";
            document.getElementById('<%=txt_stf_indicc.ClientID %>').value = "";
            document.getElementById('<%=txt_stf_indicc1.ClientID %>').value = "";
            document.getElementById('<%=txt_stf_subj.ClientID %>').value = "";
            document.getElementById('<%=txt_doc.ClientID %>').value = "";
            document.getElementById('<%=txt_deliv.ClientID %>').value = "";
            document.getElementById('<%=txt_stf_refno.ClientID %>').value = "";
            document.getElementById('<%=txt_dept.ClientID %>').value = "";
            document.getElementById('<%=txt_desigtn.ClientID %>').value = "";
            document.getElementById('<%=txt_stafftype.ClientID %>').value = "";
            document.getElementById('<%=txt_stf_add.ClientID %>').value = "";
            document.getElementById('<%=txt_stf_str.ClientID %>').value = "";
            document.getElementById('<%=txt_stf_city.ClientID %>').value = "";
           
            document.getElementById('<%=txt_stf_pin.ClientID %>').value = "";
            document.getElementById('<%=txt_chargesstaff.ClientID %>').value = "";
            document.getElementById('<%=txt_staff_chequename.ClientID %>').value = "";
            document.getElementById('<%=txt_staff_cheque.ClientID %>').value = "";
            document.getElementById('<%=txt_staff_chequeamt.ClientID %>').value = "";
            document.getElementById('<%=txt_staffddname.ClientID %>').value = "";
            document.getElementById('<%=txt_staff_ddno.ClientID %>').value = "";
            document.getElementById('<%=txt_staff_ddamt.ClientID %>').value = "";
            document.getElementById('<%=cb_paymentstaff.ClientID %>').checked = false;
            //cb_paymentstaff.checked = false;
            return false;

        }
        //********************* parent div clear **********************

        function par_div_clear() {
            parent_div_clear();

            var indiv = document.getElementById("<%=div_individu.ClientID %>");
            indiv.style.display = "none";
        }

        function parent_div_clear() {
            ddl_clear_par();
            document.getElementById('<%=txt_rollno.ClientID %>').value = "";
            document.getElementById('<%=txt_studname.ClientID %>').value = "";
            document.getElementById('<%=txt_fname.ClientID %>').value = "";
            document.getElementById('<%=txt_mob.ClientID %>').value = "";
            document.getElementById('<%=txt_addr.ClientID %>').value = "";
            document.getElementById('<%=txt_street2.ClientID %>').value = "";
            document.getElementById('<%=txt_par_city.ClientID %>').value = "";
            document.getElementById('<%=txt_par_state.ClientID %>').value = "";
            document.getElementById('<%=txt_par_country.ClientID %>').value = "";
            document.getElementById('<%=txt_par_phno.ClientID %>').value = "";
            document.getElementById('<%=txt_par_mail.ClientID %>').value = "";
            document.getElementById('<%=txt_par_depto.ClientID %>').value = "";
            document.getElementById('<%=txt_par_depto1.ClientID %>').value = "";
            document.getElementById('<%=txt_par_cc.ClientID %>').value = "";
            document.getElementById('<%=txt_par_cc1.ClientID %>').value = "";
            document.getElementById('<%=txt_par_indi.ClientID %>').value = "";
            document.getElementById('<%=txt_par_indi1.ClientID %>').value = "";
            document.getElementById('<%=txt_par_cc2.ClientID %>').value = "";
            document.getElementById('<%=txt_par_cc3.ClientID %>').value = "";
            document.getElementById('<%=txt_par_studto.ClientID %>').value = "";
            document.getElementById('<%=txt_par_studto1.ClientID %>').value = "";
            document.getElementById('<%=txt_par_sub.ClientID %>').value = "";
            document.getElementById('<%=txt_par_doc1.ClientID %>').value = "";
            document.getElementById('<%=txt_par_deliv1.ClientID %>').value = "";
            document.getElementById('<%=txt_par_refno.ClientID %>').value = "";
            document.getElementById('<%=FileUpload_parnt.ClientID %>').value = "";
            document.getElementById('<%=txt_par_pin.ClientID %>').value = "";
            document.getElementById("errmsg").innerHTML = "";
            document.getElementById('<%=txt_chargespar.ClientID %>').value = "";
            document.getElementById('<%=txt_par_chq_name.ClientID %>').value = "";
            document.getElementById('<%=txt_par_chq_no.ClientID %>').value = "";
            document.getElementById('<%=txt_par_chq_amt.ClientID %>').value = "";
            document.getElementById('<%=txt_par_ddname.ClientID %>').value = "";
            document.getElementById('<%=txt_parddno.ClientID %>').value = "";
            document.getElementById('<%=txt_par_ddamt.ClientID %>').value = "";

            document.getElementById('<%=cb_paymentpar.ClientID %>').checked = false;
            //cb_paymentpar.checked = false;
            return false;
        }
        // ************************************ company div clear ******************************

        function div_company_clear() {
            ddl_clear_com();
            document.getElementById('<%=txt_cname.ClientID %>').value = "";
            document.getElementById('<%=txt_pername.ClientID %>').value = "";
            document.getElementById('<%=txt_des.ClientID %>').value = "";
            document.getElementById('<%=txt_com_add.ClientID %>').value = "";
            document.getElementById('<%=txt_com_str.ClientID %>').value = "";
            document.getElementById('<%=txt_com_city.ClientID %>').value = "";
            document.getElementById('<%=txt_com_state.ClientID %>').value = "";
            document.getElementById('<%=txt_com_county.ClientID %>').value = "";
            document.getElementById('<%=txt_com_ph.ClientID %>').value = "";
            document.getElementById('<%=txt_par_refno.ClientID %>').value = "";
            document.getElementById('<%=txt_com_mail.ClientID %>').value = "";
            document.getElementById('<%=txt_comdepto.ClientID %>').value = "";
            document.getElementById('<%=txt_comdepto1.ClientID %>').value = "";
            document.getElementById('<%=txt_comcc.ClientID %>').value = "";
            document.getElementById('<%=txt_comcc2.ClientID %>').value = "";
            document.getElementById('<%=txt_com_ind.ClientID %>').value = "";
            document.getElementById('<%=txt_com_ind1.ClientID %>').value = "";
            document.getElementById('<%=txt_com_cc.ClientID %>').value = "";
            document.getElementById('<%=txt_com_cc1.ClientID %>').value = "";
            document.getElementById('<%=txt_com_sub.ClientID %>').value = "";
            document.getElementById('<%=txt_com_doc1.ClientID %>').value = "";
            document.getElementById('<%=txt_com_deliv1.ClientID %>').value = "";
            document.getElementById('<%=txt_com_refno.ClientID %>').value = "";
            document.getElementById('<%=txt_stud_doc_others.ClientID %>').value = "";
            document.getElementById('<%=txt_stud_delivery.ClientID %>').value = "";
           
           
            document.getElementById('<%=txt_com_pin.ClientID %>').value = "";
            document.getElementById('<%=txt_chargecomp.ClientID %>').value = "";
            document.getElementById('<%=txt_comp_chqname.ClientID %>').value = "";
            document.getElementById('<%=txt_comp_chqno.ClientID %>').value = "";
            document.getElementById('<%=txt_comp_chqamt.ClientID %>').value = "";
            document.getElementById('<%=txt_compddname.ClientID %>').value = "";
            document.getElementById('<%=txt_compddno.ClientID %>').value = "";
            document.getElementById('<%=txt_comp_ddamt.ClientID %>').value = "";
            //cb_paymentcomp.checked = false;
            document.getElementById('<%=cb_paymentcomp.ClientID %>').checked = false;
            return false;
        }

        //*************************** individual div clear *****************************

        function div_individual_clear() {
            ddl_clear_indi();
            document.getElementById('<%=txt_ind_cname.ClientID %>').value = "";
            document.getElementById('<%=txt_ind_pin.ClientID %>').value = "";
            document.getElementById('<%=txt_ind_pname.ClientID %>').value = "";
            document.getElementById('<%=txt_ind_des.ClientID %>').value = "";
            document.getElementById('<%=txt_ind_add.ClientID %>').value = "";
            document.getElementById('<%=txt_ind_str.ClientID %>').value = "";
            document.getElementById('<%=txt_ind_city.ClientID %>').value = "";
            document.getElementById('<%=txt_ind_state.ClientID %>').value = "";
            document.getElementById('<%=txt_ind_county.ClientID %>').value = "";
            document.getElementById('<%=txt_ind_ph.ClientID %>').value = "";
            document.getElementById('<%=txt_ind_mail.ClientID %>').value = "";
            document.getElementById('<%=txt_ind_depto.ClientID %>').value = "";
            document.getElementById('<%=txt_ind_depto1.ClientID %>').value = "";
            document.getElementById('<%=txt_ind_cc.ClientID %>').value = "";
            document.getElementById('<%=txt_ind_cc1.ClientID %>').value = "";
            document.getElementById('<%=txt_ind_indivto.ClientID %>').value = "";
            document.getElementById('<%=txt_ind_indiv1.ClientID %>').value = "";
            document.getElementById('<%=txt_ind_indivcc.ClientID %>').value = "";
            document.getElementById('<%=txt_ind_indivcc1.ClientID %>').value = "";
            document.getElementById('<%=txt_ind_sub.ClientID %>').value = "";
            document.getElementById('<%=txt_ind_doc1.ClientID %>').value = "";
            document.getElementById('<%=txt_ind_deliv1.ClientID %>').value = "";
            document.getElementById('<%=txt_ind_refno.ClientID %>').value = "";
            document.getElementById('<%=FileUpload_ind.ClientID %>').value = "";
            document.getElementById('<%=txt_chargesindi.ClientID %>').value = "";
            document.getElementById('<%=txt_indi_chqname.ClientID %>').value = "";
            document.getElementById('<%=txt_indi_chqno.ClientID %>').value = "";
            document.getElementById('<%=txt_indi_chqamt.ClientID %>').value = "";
            document.getElementById('<%=txt_comddname.ClientID %>').value = "";
            document.getElementById('<%=txt_indiddno.ClientID %>').value = "";
            document.getElementById('<%=txt_indi_dd_amt.ClientID %>').value = "";
            //cb_paymentindi.checked = false;
            document.getElementById('<%=cb_paymentindi.ClientID %>').checked = false;
            return false;
        }

        //*********************************** magazine div clear ************************

        function div_mag_clear() {
            ddl_clear_mag();
            document.getElementById('<%=txt_magzname.ClientID %>').value = "";
            document.getElementById('<%=txt_magztype.ClientID %>').value = "";
            document.getElementById('<%=txt_magz_add.ClientID %>').value = "";
            document.getElementById('<%=txt_magz_str.ClientID %>').value = "";
            document.getElementById('<%=txt_magz_city.ClientID %>').value = "";
            document.getElementById('<%=txt_magz_state.ClientID %>').value = "";
            document.getElementById('<%=txt_magz_county.ClientID %>').value = "";
            document.getElementById('<%=txt_magz_ph.ClientID %>').value = "";
            document.getElementById('<%=txt_pin.ClientID %>').value = "";

            document.getElementById('<%=txt_magz_mail.ClientID %>').value = "";
            document.getElementById('<%=txt_magz_depto.ClientID %>').value = "";
            document.getElementById('<%=txt_magz_depto1.ClientID %>').value = "";
            document.getElementById('<%=txt_magz_cc.ClientID %>').value = "";
            document.getElementById('<%=txt_magz_cc1.ClientID %>').value = "";
            document.getElementById('<%=txt_magz_indi.ClientID %>').value = "";
            document.getElementById('<%=txt_magz_indi1.ClientID %>').value = "";
            document.getElementById('<%=txt_magz_cc2.ClientID %>').value = "";
            document.getElementById('<%=txt_magz_cc3.ClientID %>').value = "";
            document.getElementById('<%=txt_magz_sub.ClientID %>').value = "";
            document.getElementById('<%=txt_magz_doc.ClientID %>').value = "";
            document.getElementById('<%=txt_magz_deliv1.ClientID %>').value = "";
            document.getElementById('<%=txt_magz_refno.ClientID %>').value = "";
            document.getElementById('<%=FileUpload_magz.ClientID %>').value = "";
            document.getElementById('<%=TextBox1.ClientID %>').value = "";
            document.getElementById('<%=txt_magchqname.ClientID %>').value = "";
            document.getElementById('<%=txt_magchno.ClientID %>').value = "";
            document.getElementById('<%=txt_mag_chqamt.ClientID %>').value = "";
            document.getElementById('<%=txt_magddname.ClientID %>').value = "";
            document.getElementById('<%=txt_magddno.ClientID %>').value = "";
            document.getElementById('<%=txt_magddamt.ClientID %>').value = "";
            //cb_paymentmag.checked = false;
            document.getElementById('<%=cb_paymentmag.ClientID %>').checked = false;
            return false;
        }

        //***************************** others div clear ***************************

        function div_others_clear() {
            ddl_clear_othr();
            document.getElementById('<%=txt_othr_name.ClientID %>').value = "";
            document.getElementById('<%=txt_othr_pname.ClientID %>').value = "";
            document.getElementById('<%=txt_othr_add.ClientID %>').value = "";
            document.getElementById('<%=txt_othr_str.ClientID %>').value = "";
            document.getElementById('<%=txt_othr_city.ClientID %>').value = "";
            document.getElementById('<%=txt_othr_state.ClientID %>').value = "";
            document.getElementById('<%=txt_othr_county.ClientID %>').value = "";
            document.getElementById('<%=txt_othr_ph.ClientID %>').value = "";
            document.getElementById('<%=txt_othr_mail.ClientID %>').value = "";
            document.getElementById('<%=txt_othr_depto.ClientID %>').value = "";
            document.getElementById('<%=txt_othr_depto1.ClientID %>').value = "";
            document.getElementById('<%=txt_othr_cc.ClientID %>').value = "";
            document.getElementById('<%=txt_othr_cc1.ClientID %>').value = "";
            document.getElementById('<%=txt_othr_indi.ClientID %>').value = "";
            document.getElementById('<%=txt_othr_indi1.ClientID %>').value = "";
            document.getElementById('<%=txt_othr_cc2.ClientID %>').value = "";
            document.getElementById('<%=txt_othr_cc3.ClientID %>').value = "";
            document.getElementById('<%=txt_othr_sub.ClientID %>').value = "";
            document.getElementById('<%=txt_othr_doc.ClientID %>').value = "";
            document.getElementById('<%=txt_othr_delivry.ClientID %>').value = "";
            document.getElementById('<%=txt_othr_refno.ClientID %>').value = "";
            
            document.getElementById('<%=txt_othr_pin.ClientID %>').value = "";
            document.getElementById('<%=txt_othersothr.ClientID %>').value = "";
            document.getElementById('<%=txt_othr_chqname.ClientID %>').value = "";
            document.getElementById('<%=txt_othr_chqno.ClientID %>').value = "";
            document.getElementById('<%=txt_othr_chqamt.ClientID %>').value = "";
            document.getElementById('<%=txt_othr_ddname.ClientID %>').value = "";
            document.getElementById('<%=txt_othr_ddno.ClientID %>').value = "";
            document.getElementById('<%=txt_othr_ddamt.ClientID %>').value = "";
            //cb_paymentothr.checked = false;
            document.getElementById('<%=cb_paymentothr.ClientID %>').checked = false;
            return false;
        }
        //*********************************  clear with in, out change**********************

        function save_stud_clear() {
            div_stud_clear();
            outchange();
            rbinfromto1();
        }

        function save_stud_clear1() {
            div_stud_clear();
            inchange();
            rbinfromto();
        }



        function save_staff_clear() {
            staff_div_clear();
            outstaff();
            rbinfromto1_staff();
        }

        function save_staff_clear1() {//delsi
            staff_div_clear();
            instaff();
            rbinfromto_staff();
            var value = document.getElementById("");
        }

        function save_parent_clear() {
            parent_div_clear();
            outparent();
            rbinfromto1_par();
        }

        function save_parent_clear1() {
            parent_div_clear();
            inparent();
            rbinfromto_par();
        }

        function save_company_clear() {
            div_company_clear();
            outcomp();
            rbinfromto1_comp();
        }

        function save_company_clear1() {
            div_company_clear();
            incomp();
            rbinfromto_comp();
        }

        function save_indi_clear() {
            div_individual_clear();
            outindi();
            rbinfromto1_indi();
        }


        function save_indi_clear1() {
            div_individual_clear();
            inindi();
            rbinfromto_indi();
        }

        function save_mag_clear() {
            div_mag_clear();
            outmag();
            rbinfromto1_mag();
        }

        function save_mag_clear1() {
            div_mag_clear();
            inmag();
            rbinfromto_mag();
        }

        function save_others_clear() {
            div_others_clear();
            outothr();
            rbinfromto1_oth();
        }


        function save_others_clear1() {
            div_others_clear();
            inothr();
            rbinfromto_oth();
        }

        //  ***********************************

        function save_alter() {
            var stud = document.getElementById("<%=imgdiv2.ClientID %>");
            stud.style.display = "none";
            return false;
        }

        function tot_alert() {


            div_stud_clear();

            staff_div_clear();

            parent_div_clear();
            div_individual_clear();
            div_mag_clear();
            div_others_clear();
            div_company_clear();
        }

        function checkEmail(id) {

            var filter = /^([a-z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            if (!filter.test(id.value)) {
                id.style.borderColor = 'Red';
                id.value = "";
                email.focus;
            }
            else {
                id.style.borderColor = '#c4c4c4';
            }
        }

        function myFunCaps(id) {
            var txt = document.getElementById(id);
            var value = txt.value;
            txt.value = value.charAt(0).toUpperCase() + value.substr(1).toLowerCase();
        }

        // ********** time**************



        //**************** stud_validation***************

        function validstud() {

            var idval = "";
            var idval1 = "";
            var idval2 = "";
            var idval3 = "";
            var idval4 = "";
            var idval5 = "";
            var empty = "";
            var ss = "";
            var sss = "";

            idval = document.getElementById("<%= txt_roll.ClientID %>").value;
            idval1 = document.getElementById("<%=txt_dept_to.ClientID %>").value;
            idval2 = document.getElementById("<%=txt_dept_cc.ClientID %>").value;
            idval3 = document.getElementById("<%=txt_subject.ClientID %>").value;
            idval4 = document.getElementById("<%=txt_indiv.ClientID %>").value;
            idval5 = document.getElementById("<%=txt_indiv_cc.ClientID %>").value;

            if (idval.trim() == "") {

                idval = document.getElementById("<%=txt_roll.ClientID %>");
                empty = "E";
                idval.style.borderColor = 'Red';
            }

            if (document.getElementById('<%=cb_dept.ClientID %>').checked == true && document.getElementById('<%=cb_individual.ClientID %>').checked == true) {
                if (idval1.trim() == "") {

                    idval1 = document.getElementById("<%=txt_dept_to.ClientID %>");
                    empty = "E";
                    idval1.style.borderColor = 'Red';
                }


                if (idval4.trim() == "") {

                    idval4 = document.getElementById("<%=txt_indiv.ClientID %>");
                    empty = "E";
                    idval4.style.borderColor = 'Red';
                }

            }

            else if (cb_dept.checked == true) {
                if (idval1.trim() == "") {

                    idval1 = document.getElementById("<%=txt_dept_to.ClientID %>");
                    empty = "E";
                    idval1.style.borderColor = 'Red';
                }

            }
            else if (cb_individual.checked == true) {
                if (idval4.trim() == "") {

                    idval4 = document.getElementById("<%=txt_indiv.ClientID %>");
                    empty = "E";
                    idval4.style.borderColor = 'Red';
                }

            }


            if (idval3.trim() == "") {
                idval3 = document.getElementById("<%=txt_subject.ClientID %>");
                empty = "E";
                idval3.style.borderColor = 'Red';
            }

            if (document.getElementById('<%=cb_dept.ClientID %>').checked == false && document.getElementById('<%=cb_individual.ClientID %>').checked == false) {

                alert("kindly select department and individual fields or anyone");
                return false;

            }



            if (empty.trim() != "") {
                return false;
            }
            else {
                return true;
            }
        }





        //**************** staff validation ******************
        function checkstaffname(txt1) {
            $.ajax({
                type: "POST",
                url: "LetterDocumentInward.aspx/Checkstfname",
                data: '{Roll_No: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccessstaffname,
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function OnSuccessstaffname(response) {
            var mesg = $("#rnomsgstaff")[0];

            switch (response.d) {
                case "0":

                    mesg.style.color = "red";
                    mesg.innerHTML = "Not exist";
                    studentclear();
                    break;
                case "1":
                    get();
                    mesg.style.color = "green";
                    mesg.innerHTML = "Available";
                    break;
                case "error":
                    mesg.style.color = "red";
                    mesg.innerHTML = "Error occurred";
                    break;
            }
        }
        //staffcode
        function checkstaffcode(txt1) {
            $.ajax({
                type: "POST",
                url: "LetterDocumentInward.aspx/checkstaffcodechk",
                data: '{Roll_No: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccessstaffcode,
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function OnSuccessstaffcode(response) {
            var mesg = $("#msgstaffcode")[0];
            switch (response.d) {
                case "0":
                    mesg.style.color = "red";
                    mesg.innerHTML = "Not exist";
                    studentclear();
                    break;
                case "1":
                    getstaffcode();
                    mesg.style.color = "green";
                    mesg.innerHTML = "Available";
                    break;
                case "error":
                    mesg.style.color = "red";
                    mesg.innerHTML = "Error occurred";
                    break;
            }
        }
        function validstaff() {
            var idval = "";
            var idval1 = "";
            var idval2 = "";
            var idval3 = "";
            var idval4 = "";
            var idval5 = "";
            var empty = "";

            idval = document.getElementById("<%= txt_name1.ClientID %>").value;
            idval1 = document.getElementById("<%=txt_stf_depto.ClientID %>").value;
            idval2 = document.getElementById("<%=txt_stf_cc.ClientID %>").value;
            idval3 = document.getElementById("<%=txt_stf_subj.ClientID %>").value;
            idval4 = document.getElementById("<%=txt_stf_indito.ClientID %>").value;
            idval5 = document.getElementById("<%=txt_stf_indicc.ClientID %>").value;

            if (idval.trim() == "") {
                idval = document.getElementById("<%=txt_name1.ClientID %>");
                idval.style.borderColor = 'Red';
                empty = "E";
            }

            if (idval3.trim() == "") {
                idval3 = document.getElementById("<%=txt_stf_subj.ClientID %>");
                idval3.style.borderColor = 'Red';
                empty = "E";
            }



            if (document.getElementById('<%=cb_stf_dept.ClientID %>').checked == true && document.getElementById('<%=cb_stf_indi.ClientID %>').checked == true) {

                if (idval1.trim() == "") {
                    idval1 = document.getElementById("<%=txt_stf_depto.ClientID %>");
                    idval1.style.borderColor = 'Red';
                    empty = "E";
                }



                if (idval4.trim() == "") {
                    idval4 = document.getElementById("<%=txt_stf_indito.ClientID %>");
                    idval4.style.borderColor = 'Red';
                    empty = "E";
                }

            }


            else if (document.getElementById('<%=cb_stf_dept.ClientID %>').checked == true) {
                if (idval1.trim() == "") {
                    idval1 = document.getElementById("<%=txt_stf_depto.ClientID %>");
                    idval1.style.borderColor = 'Red';
                    empty = "E";
                }


            }

            else if (document.getElementById('<%=cb_stf_indi.ClientID %>').checked == true) {

                if (idval4.trim() == "") {
                    idval4 = document.getElementById("<%=txt_stf_indito.ClientID %>");
                    idval4.style.borderColor = 'Red';
                    empty = "E";
                }

            }

            if (document.getElementById('<%=cb_stf_dept.ClientID %>').checked == false && document.getElementById('<%=cb_stf_indi.ClientID %>').checked == false) {
                alert("Kindly select department and individual fields or Anyone");
                return false;
            }

            if (empty.trim() != "") {
                return false;
            }
            else {
                return true;
            }

        }



        function checkreportsave(caller) {


            var fromdate = document.getElementById("<%=txt_fromdate.ClientID %>").value;
            var todate = document.getElementById("<%=txt_todate.ClientID %>").value;
            if (fromdate.trim().toUpperCase() == "" || fromdate.trim().toUpperCase() == "00/00/0000" && todate.trim().toUpperCase() == "" || todate.trim().toUpperCase() == "00/00/0000") {

                err += checkEmpty(document.getElementById("<%=txt_fromdate.ClientID %>"), "Enter From Date");
                err += checkEmpty(document.getElementById("<%=txt_todate.ClientID %>"), "Enter To Date");
            }


            
        }

        function validpar() {
            var idval = "";
            var idval1 = "";
            var idval2 = "";
            var idval3 = "";
            var idval4 = "";
            var idval5 = "";
            var empty = "";

            idval = document.getElementById("<%= txt_fname.ClientID %>").value;
            idval1 = document.getElementById("<%=txt_par_sub.ClientID %>").value;
            idval2 = document.getElementById("<%=txt_par_depto.ClientID %>").value;
            idval3 = document.getElementById("<%=txt_par_cc.ClientID %>").value;
            idval4 = document.getElementById("<%=txt_par_indi.ClientID %>").value;
            idval5 = document.getElementById("<%=txt_par_cc2.ClientID %>").value;

            if (idval.trim() == "") {
                idval = document.getElementById("<%=txt_fname.ClientID %>");
                idval.style.borderColor = 'Red';
                empty = "E";
            }


            if (idval1.trim() == "") {
                idval1 = document.getElementById("<%=txt_par_sub.ClientID %>");
                idval1.style.borderColor = 'Red';
                empty = "E";
            }

            if (document.getElementById('<%=cb_par_dept.ClientID %>').checked == true && document.getElementById('<%=cb_par_indi.ClientID %>').checked == true) {
                if (idval2.trim() == "") {
                    idval2 = document.getElementById("<%=txt_par_depto.ClientID %>");
                    idval2.style.borderColor = 'Red';
                    empty = "E";
                }



                if (idval4.trim() == "") {
                    idval4 = document.getElementById("<%=txt_par_indi.ClientID %>");
                    idval4.style.borderColor = 'Red';
                    empty = "E";
                }


            }
            else if (document.getElementById('<%=cb_par_dept.ClientID %>').checked == true) {
                if (idval2.trim() == "") {
                    idval2 = document.getElementById("<%=txt_par_depto.ClientID %>");
                    idval2.style.borderColor = 'Red';
                    empty = "E";
                }


            }
            else if (document.getElementById('<%=cb_par_indi.ClientID %>').checked == true) {
                if (idval4.trim() == "") {
                    idval4 = document.getElementById("<%=txt_par_indi.ClientID %>");
                    idval4.style.borderColor = 'Red';
                    empty = "E";
                }


            }

            if (document.getElementById('<%=cb_par_dept.ClientID %>').checked == false && document.getElementById('<%=cb_par_indi.ClientID %>').checked == false) {
                alert("Kindly select department and individual fields or Anyone");
                return false;
            }


            if (empty.trim() != "") {
                return false;
            }
            else {
                return true;
            }

        }

        function validcomp() {
            var idval = "";
            var idval1 = "";
            var idval2 = "";
            var idval3 = "";
            var idval4 = "";
            var idval5 = "";
            var empty = "";

            idval = document.getElementById("<%= txt_cname.ClientID %>").value;
            idval1 = document.getElementById("<%=txt_com_sub.ClientID %>").value;
            idval2 = document.getElementById("<%=txt_comdepto.ClientID %>").value;

            idval4 = document.getElementById("<%=txt_com_ind.ClientID %>").value;

            if (idval.trim() == "") {
                idval = document.getElementById("<%=txt_cname.ClientID %>");
                idval.style.borderColor = 'Red';
                empty = "E";
            }
            if (idval1.trim() == "") {
                idval1 = document.getElementById("<%=txt_com_sub.ClientID %>");
                idval1.style.borderColor = 'Red';
                empty = "E";
            }
            if (idval2.trim() == "") {
                idval2 = document.getElementById("<%=txt_comdepto.ClientID %>");
                idval2.style.borderColor = 'Red';
                empty = "E";
            }


            if (document.getElementById('<%=cb_com_indi.ClientID %>').checked == true) {
                if (idval4.trim() == "") {
                    idval4 = document.getElementById("<%=txt_com_ind.ClientID %>");
                    idval4.style.borderColor = 'Red';
                    empty = "E";
                }


            }

            if (document.getElementById('<%=cb_com_dept.ClientID %>').checked == false && document.getElementById('<%=cb_com_indi.ClientID %>').checked == false) {
                alert("Kindly select department and individual fields or Anyone");
                return false;
            }

            if (empty.trim() != "") {
                return false;
            }
            else {
                return true;
            }

        }


        function validindi() {
            var idval = "";
            var idval1 = "";
            var idval2 = "";
            var idval3 = "";
            var idval4 = "";
            var idval5 = "";
            var empty = "";

            idval = document.getElementById("<%= txt_ind_cname.ClientID %>").value;
            idval1 = document.getElementById("<%=txt_ind_sub.ClientID %>").value;
            idval2 = document.getElementById("<%=txt_ind_depto.ClientID %>").value;
            idval3 = document.getElementById("<%=txt_ind_cc.ClientID %>").value;
            idval4 = document.getElementById("<%=txt_ind_indivto.ClientID %>").value;
            idval5 = document.getElementById("<%=txt_ind_indivcc.ClientID %>").value;


            if (idval.trim() == "") {
                idval = document.getElementById("<%=txt_ind_cname.ClientID %>");
                idval.style.borderColor = 'Red';
                empty = "E";
            }

            if (idval1.trim() == "") {
                idval1 = document.getElementById("<%=txt_ind_sub.ClientID %>");
                idval1.style.borderColor = 'Red';
                empty = "E";
            }
            if (document.getElementById('<%=cb_ind_dept.ClientID %>').checked == true && document.getElementById('<%=cb_ind_indi.ClientID %>').checked == true) {
                if (idval2.trim() == "") {
                    idval2 = document.getElementById("<%=txt_ind_depto.ClientID %>");
                    idval2.style.borderColor = 'Red';
                    empty = "E";
                }

                if (idval4.trim() == "") {
                    idval4 = document.getElementById("<%=txt_ind_indivto.ClientID %>");
                    idval4.style.borderColor = 'Red';
                    empty = "E";
                }

            }

            else if (document.getElementById('<%=cb_ind_dept.ClientID %>').checked == true) {
                if (idval2.trim() == "") {
                    idval2 = document.getElementById("<%=txt_ind_depto.ClientID %>");
                    idval2.style.borderColor = 'Red';
                    empty = "E";
                }

            }
            else if (document.getElementById('<%=cb_ind_indi.ClientID %>').checked == true) {
                if (idval4.trim() == "") {
                    idval4 = document.getElementById("<%=txt_ind_indivto.ClientID %>");
                    idval4.style.borderColor = 'Red';
                    empty = "E";
                }

            }

            if (document.getElementById('<%=cb_ind_dept.ClientID %>').checked == false && document.getElementById('<%=cb_ind_indi.ClientID %>').checked == false) {
                alert("Kindly select department and individual fields or Anyone");
                return false;
            }

            if (empty.trim() != "") {
                return false;
            }
            else {
                return true;
            }

        }

        function validmag() {
            var idval = "";
            var idval1 = "";
            var idval2 = "";
            var idval3 = "";
            var idval4 = "";
            var idval5 = "";
            var empty = "";

            idval = document.getElementById("<%= txt_magzname.ClientID %>").value;
            idval1 = document.getElementById("<%=txt_magz_sub.ClientID %>").value;
            idval2 = document.getElementById("<%=txt_magz_depto.ClientID %>").value;
            idval3 = document.getElementById("<%=txt_magz_cc.ClientID %>").value;
            idval4 = document.getElementById("<%=txt_magz_indi.ClientID %>").value;
            idval5 = document.getElementById("<%=txt_magz_cc2.ClientID %>").value;

            if (idval.trim() == "") {
                idval = document.getElementById("<%=txt_magzname.ClientID %>");
                idval.style.borderColor = 'Red';
                empty = "E";
            }

            if (idval1.trim() == "") {
                idval1 = document.getElementById("<%=txt_magz_sub.ClientID %>");
                idval1.style.borderColor = 'Red';
                empty = "E";
            }

            if (document.getElementById('<%=cb_magz_dept.ClientID %>').checked == true && document.getElementById('<%=cb_magz_indi.ClientID %>').checked == true) {
                if (idval2.trim() == "") {
                    idval2 = document.getElementById("<%=txt_magz_depto.ClientID %>");
                    idval2.style.borderColor = 'Red';
                    empty = "E";
                }

                if (idval4.trim() == "") {
                    idval4 = document.getElementById("<%=txt_magz_indi.ClientID %>");
                    idval4.style.borderColor = 'Red';
                    empty = "E";
                }

            }

            else if (document.getElementById('<%=cb_magz_dept.ClientID %>').checked == true) {
                if (idval2.trim() == "") {
                    idval2 = document.getElementById("<%=txt_magz_depto.ClientID %>");
                    idval2.style.borderColor = 'Red';
                    empty = "E";
                }

            }
            else if (document.getElementById('<%=cb_magz_indi.ClientID %>').checked == true) {
                if (idval4.trim() == "") {
                    idval4 = document.getElementById("<%=txt_magz_indi.ClientID %>");
                    idval4.style.borderColor = 'Red';
                    empty = "E";
                }

            }

            if (document.getElementById('<%=cb_magz_dept.ClientID %>').checked == false && document.getElementById('<%=cb_magz_indi.ClientID %>').checked == false) {
                alert("Kindly select department and individual fields or Anyone");
                return false;
            }
            if (empty.trim() != "") {
                return false;
            }
            else {
                return true;
            }

        }


        function validothr() {
            var idval = "";
            var idval = "";
            var idval1 = "";
            var idval2 = "";
            var idval3 = "";
            var idval4 = "";
            var idval5 = "";
            var empty = "";

            idval = document.getElementById("<%= txt_othr_name.ClientID %>").value;
            idval1 = document.getElementById("<%=txt_othr_sub.ClientID %>").value;
            idval2 = document.getElementById("<%=txt_othr_depto.ClientID %>").value;
            idval3 = document.getElementById("<%=txt_othr_cc.ClientID %>").value;
            idval4 = document.getElementById("<%=txt_othr_indi.ClientID %>").value;
            idval5 = document.getElementById("<%=txt_othr_cc2.ClientID %>").value;

            if (idval.trim() == "") {
                idval = document.getElementById("<%=txt_othr_name.ClientID %>");
                idval.style.borderColor = 'Red';
                empty = "E";
            }

            if (idval1.trim() == "") {
                idval1 = document.getElementById("<%=txt_othr_sub.ClientID %>");
                idval1.style.borderColor = 'Red';
                empty = "E";
            }

            if (document.getElementById('<%=cb_othr_dept.ClientID %>').checked == true && document.getElementById('<%=cb_othr_indi.ClientID %>').checked == true) {
                if (idval2.trim() == "") {
                    idval2 = document.getElementById("<%=txt_othr_depto.ClientID %>");
                    idval2.style.borderColor = 'Red';
                    empty = "E";
                }


                if (idval4.trim() == "") {
                    idval4 = document.getElementById("<%=txt_othr_indi.ClientID %>");
                    idval4.style.borderColor = 'Red';
                    empty = "E";
                }



            }
            else if (document.getElementById('<%=cb_othr_dept.ClientID %>').checked == true) {
                if (idval2.trim() == "") {
                    idval2 = document.getElementById("<%=txt_othr_depto.ClientID %>");
                    idval2.style.borderColor = 'Red';
                    empty = "E";
                }


            }
            else if (document.getElementById('<%=cb_othr_indi.ClientID %>').checked == true) {
                if (idval4.trim() == "") {
                    idval4 = document.getElementById("<%=txt_othr_indi.ClientID %>");
                    idval4.style.borderColor = 'Red';
                    empty = "E";
                }


            }

            if (document.getElementById('<%=cb_othr_dept.ClientID %>').checked == false && document.getElementById('<%=cb_othr_indi.ClientID %>').checked == false) {
                alert("Kindly select department and individual fields or Anyone");
                return false;

            }
            if (empty.trim() != "") {
                return false;
            }
            else {
                return true;
            }


        }
        //        .................payment.....................




        function chequechange() {
            if (document.getElementById('<%=rdo_cheque.ClientID %>').checked == true) {
                var idval = document.getElementById("<%=div_cheque.ClientID %>");
                idval.style.display = "block";
                document.getElementById('<%=txt_ddname.ClientID %>').value = "";
                document.getElementById('<%=txt_ddno.ClientID %>').value = "";
                document.getElementById('<%=txt_ddamt.ClientID %>').value = "";
                var idval1 = document.getElementById("<%=div_dd.ClientID %>");
                idval1.style.display = "none";
                return false;
            }
            else {
                var idval = document.getElementById("<%=div_cheque.ClientID %>");
                idval.style.display = "none";
                return false;
            }
        }

        function ddchange() {
            if (document.getElementById('<%=rdo_dd.ClientID %>').checked == true) {
                var idval = document.getElementById("<%=div_dd.ClientID %>");
                idval.style.display = "block";
                document.getElementById('<%=txt_chequename.ClientID %>').value = "";
                document.getElementById('<%=txt_chequenumb.ClientID %>').value = "";
                document.getElementById('<%=txt_chequeamt.ClientID %>').value = "";
                var idval1 = document.getElementById("<%=div_cheque.ClientID %>");
                idval1.style.display = "none";
                return false;
            }
            else {
                var idval = document.getElementById("<%=div_dd.ClientID %>");
                idval.style.display = "none";
                return false;
            }
        }
        //        delsi1901
        function studpayment() {
            if (document.getElementById('<%=cb_paymentstud.ClientID %>').checked == true) {
                var idval = document.getElementById("<%=div_stud_payment.ClientID %>");
                idval.style.display = "block";
                document.getElementById('<%=rdo_cheque.ClientID %>').checked = true;
                var idval1 = document.getElementById("<%=div_cheque.ClientID %>");
                idval1.style.display = "block";
                var idval2 = document.getElementById("<%=div_dd.ClientID %>");
                idval2.style.display = "none";
                return false;
            }
            else {
                var idval = document.getElementById("<%=div_stud_payment.ClientID %>");
                idval.style.display = "none";
                var idval1 = document.getElementById("<%=div_cheque.ClientID %>");
                idval1.style.display = "none";
                var idval2 = document.getElementById("<%=div_dd.ClientID %>");
                idval2.style.display = "none";
                return false;
            }
                }


function studpayment1() {
    if (document.getElementById('<%=chk_stud_charges.ClientID %>').checked == true) {
        var idval = document.getElementById("<%=div_student_payment.ClientID %>");
                idval.style.display = "block";
                document.getElementById('<%=rdo_stud_cheque.ClientID %>').checked = true;
                var idval1 = document.getElementById("<%=div_stud_cheque.ClientID %>");
                idval1.style.display = "block";
                var idval2 = document.getElementById("<%=div_student_dd.ClientID %>");
                idval2.style.display = "none";
                return false;
            }
            else {
                var idval = document.getElementById("<%=div_student_payment.ClientID %>");
                idval.style.display = "none";
                var idval1 = document.getElementById("<%=div_stud_cheque.ClientID %>");
                idval1.style.display = "none";
                var idval2 = document.getElementById("<%=div_student_dd.ClientID %>");
                idval2.style.display = "none";
                return false;
            }
        }
        function staffpayment() {
            if (document.getElementById('<%=cb_paymentstaff.ClientID %>').checked == true) {
                var idval = document.getElementById("<%=div_staff_payment.ClientID %>");
                idval.style.display = "block";
                document.getElementById('<%=rdo_cheque.ClientID %>').checked = true;
                var idval1 = document.getElementById("<%=div_staff_cheque.ClientID %>");
                idval1.style.display = "block";
                var idval2 = document.getElementById("<%=div_staff_dd.ClientID %>");
                idval2.style.display = "none";
                return false;
            }
            else {
                var idval = document.getElementById("<%=div_staff_payment.ClientID %>");
                idval.style.display = "none";
                var idval1 = document.getElementById("<%=div_staff_cheque.ClientID %>");
                idval1.style.display = "none";
                var idval2 = document.getElementById("<%=div_staff_dd.ClientID %>");
                idval2.style.display = "none";
                return false;
            }
        }



        function parentpayment() {
            if (document.getElementById('<%=cb_paymentpar.ClientID %>').checked == true) {
                var idval = document.getElementById("<%=div_par_payment.ClientID %>");
                idval.style.display = "block";
                document.getElementById('<%=rdo_cheque.ClientID %>').checked = true;
                var idval1 = document.getElementById("<%=div_par_cheque.ClientID %>");
                idval1.style.display = "block";
                var idval2 = document.getElementById("<%=div_par_dd.ClientID %>");
                idval2.style.display = "none";
                return false;
            }
            else {
                var idval = document.getElementById("<%=div_par_payment.ClientID %>");
                idval.style.display = "none";
                var idval1 = document.getElementById("<%=div_par_cheque.ClientID %>");
                idval1.style.display = "none";
                var idval2 = document.getElementById("<%=div_par_dd.ClientID %>");
                idval2.style.display = "none";
                return false;
            }
        }
        function comppayment() {
            if (document.getElementById('<%=cb_paymentcomp.ClientID %>').checked == true) {
                var idval = document.getElementById("<%=div_comp_payment.ClientID %>");
                idval.style.display = "block";
                document.getElementById('<%=rdo_cheque.ClientID %>').checked = true;
                var idval1 = document.getElementById("<%=div_comp_cheque.ClientID %>");
                idval1.style.display = "block";
                var idval2 = document.getElementById("<%=div_comp_dd.ClientID %>");
                idval2.style.display = "none";
                return false;
            }
            else {
                var idval = document.getElementById("<%=div_comp_payment.ClientID %>");
                idval.style.display = "none";
                var idval1 = document.getElementById("<%=div_comp_cheque.ClientID %>");
                idval1.style.display = "none";
                var idval2 = document.getElementById("<%=div_comp_dd.ClientID %>");
                idval2.style.display = "none";
                return false;
            }
        }

        function instpayment() {
            if (document.getElementById('<%=cb_paymentindi.ClientID %>').checked == true) {
                var idval = document.getElementById("<%=div_indi_payment.ClientID %>");
                idval.style.display = "block";
                document.getElementById('<%=rdo_cheque.ClientID %>').checked = true;
                var idval1 = document.getElementById("<%=div_indi_cheque.ClientID %>");
                idval1.style.display = "block";
                var idval2 = document.getElementById("<%=div_indi_dd.ClientID %>");
                idval2.style.display = "none";
                return false;
            }
            else {
                var idval = document.getElementById("<%=div_indi_payment.ClientID %>");
                idval.style.display = "none";
                var idval1 = document.getElementById("<%=div_indi_cheque.ClientID %>");
                idval1.style.display = "none";
                var idval2 = document.getElementById("<%=div_indi_dd.ClientID %>");
                idval2.style.display = "none";
                return false;
            }
        }
        function magpayment() {
            if (document.getElementById('<%=cb_paymentmag.ClientID %>').checked == true) {
                var idval = document.getElementById("<%=div_mag_payment.ClientID %>");
                idval.style.display = "block";
                document.getElementById('<%=rdo_cheque.ClientID %>').checked = true;
                var idval1 = document.getElementById("<%=div_mag_cheque.ClientID %>");
                idval1.style.display = "block";
                var idval2 = document.getElementById("<%=div_mag_dd.ClientID %>");
                idval2.style.display = "none";
                return false;
            }
            else {
                var idval = document.getElementById("<%=div_mag_payment.ClientID %>");
                idval.style.display = "none";
                var idval1 = document.getElementById("<%=div_mag_cheque.ClientID %>");
                idval1.style.display = "none";
                var idval2 = document.getElementById("<%=div_mag_dd.ClientID %>");
                idval2.style.display = "none";
                return false;
            }
        }
        function otherpayment() {
            if (document.getElementById('<%=cb_paymentothr.ClientID %>').checked == true) {
                var idval = document.getElementById("<%=div_othr_payment.ClientID %>");
                idval.style.display = "block";
                document.getElementById('<%=rdo_cheque.ClientID %>').checked = true;
                var idval1 = document.getElementById("<%=div_othr_cheque.ClientID %>");
                idval1.style.display = "block";
                var idval2 = document.getElementById("<%=div_othr_dd.ClientID %>");
                idval2.style.display = "none";
                return false;
            }
            else {
                var idval = document.getElementById("<%=div_othr_payment.ClientID %>");
                idval.style.display = "none";
                var idval1 = document.getElementById("<%=div_othr_cheque.ClientID %>");
                idval1.style.display = "none";
                var idval2 = document.getElementById("<%=div_othr_dd.ClientID %>");
                idval2.style.display = "none";
                return false;
            }
        }
        //        ____________________________________cheque dd change_______________________-------

        function chequechangestaff() {
            if (document.getElementById('<%=rdb_staff_cheque.ClientID %>').checked == true) {
                var idval = document.getElementById("<%=div_staff_cheque.ClientID %>");
                idval.style.display = "block";
                document.getElementById('<%=txt_ddname.ClientID %>').value = "";
                document.getElementById('<%=txt_ddno.ClientID %>').value = "";
                document.getElementById('<%=txt_ddamt.ClientID %>').value = "";
                var idval1 = document.getElementById("<%=div_staff_dd.ClientID %>");
                idval1.style.display = "none";
                return false;
            }
            else {
                var idval = document.getElementById("<%=div_staff_cheque.ClientID %>");
                idval.style.display = "none";
                return false;
            }
        }

        function ddchangestaff() {
            if (document.getElementById('<%=rdb_staff_dd.ClientID %>').checked == true) {

                var idval = document.getElementById("<%=div_staff_dd.ClientID %>");
                idval.style.display = "block";

                document.getElementById('<%=txt_chequename.ClientID %>').value = "";
                document.getElementById('<%=txt_chequenumb.ClientID %>').value = "";
                document.getElementById('<%=txt_chequeamt.ClientID %>').value = "";
                var idval1 = document.getElementById("<%=div_staff_cheque.ClientID %>");
                idval1.style.display = "none";
                return false;
            }
            else {
                var idval = document.getElementById("<%=div_dd.ClientID %>");
                idval.style.display = "none";
                return false;
            }
        }
        function chequechangepar() {
            if (document.getElementById('<%=rdo_par_cheque.ClientID %>').checked == true) {
                var idval = document.getElementById("<%=div_par_cheque.ClientID %>");
                idval.style.display = "block";
                document.getElementById('<%=txt_ddname.ClientID %>').value = "";
                document.getElementById('<%=txt_ddno.ClientID %>').value = "";
                document.getElementById('<%=txt_ddamt.ClientID %>').value = "";
                var idval1 = document.getElementById("<%=div_par_dd.ClientID %>");
                idval1.style.display = "none";
                return false;
            }
            else {
                var idval = document.getElementById("<%=div_par_cheque.ClientID %>");
                idval.style.display = "none";
                return false;
            }
        }

        function ddchangepar() {
            if (document.getElementById('<%=rdo_pardd.ClientID %>').checked == true) {

                var idval = document.getElementById("<%=div_par_dd.ClientID %>");
                idval.style.display = "block";

                document.getElementById('<%=txt_chequename.ClientID %>').value = "";
                document.getElementById('<%=txt_chequenumb.ClientID %>').value = "";
                document.getElementById('<%=txt_chequeamt.ClientID %>').value = "";
                var idval1 = document.getElementById("<%=div_par_cheque.ClientID %>");
                idval1.style.display = "none";
                return false;
            }
            else {
                var idval = document.getElementById("<%=div_par_dd.ClientID %>");
                idval.style.display = "none";
                return false;
            }
        }
        function chequechangecomp() {
            if (document.getElementById('<%=rdo_comp_cheque.ClientID %>').checked == true) {
                var idval = document.getElementById("<%=div_comp_cheque.ClientID %>");
                idval.style.display = "block";
                document.getElementById('<%=txt_ddname.ClientID %>').value = "";
                document.getElementById('<%=txt_ddno.ClientID %>').value = "";
                document.getElementById('<%=txt_ddamt.ClientID %>').value = "";
                var idval1 = document.getElementById("<%=div_comp_dd.ClientID %>");
                idval1.style.display = "none";
                return false;
            }
            else {
                var idval = document.getElementById("<%=div_comp_cheque.ClientID %>");
                idval.style.display = "none";
                return false;
            }
        }

        function ddchangecomp() {
            if (document.getElementById('<%=rdo_comp_chedd.ClientID %>').checked == true) {

                var idval = document.getElementById("<%=div_comp_dd.ClientID %>");
                idval.style.display = "block";

                document.getElementById('<%=txt_chequename.ClientID %>').value = "";
                document.getElementById('<%=txt_chequenumb.ClientID %>').value = "";
                document.getElementById('<%=txt_chequeamt.ClientID %>').value = "";
                var idval1 = document.getElementById("<%=div_comp_cheque.ClientID %>");
                idval1.style.display = "none";
                return false;
            }
            else {
                var idval = document.getElementById("<%=div_comp_dd.ClientID %>");
                idval.style.display = "none";
                return false;
            }
        }
        function chequechangeindi() {
            if (document.getElementById('<%=rdb_indi_cheque.ClientID %>').checked == true) {
                var idval = document.getElementById("<%=div_indi_cheque.ClientID %>");
                idval.style.display = "block";
                document.getElementById('<%=txt_ddname.ClientID %>').value = "";
                document.getElementById('<%=txt_ddno.ClientID %>').value = "";
                document.getElementById('<%=txt_ddamt.ClientID %>').value = "";
                var idval1 = document.getElementById("<%=div_indi_dd.ClientID %>");
                idval1.style.display = "none";
                return false;
            }
            else {
                var idval = document.getElementById("<%=div_indi_cheque.ClientID %>");
                idval.style.display = "none";
                return false;
            }
        }

        function ddchangeindi() {
            if (document.getElementById('<%=rdb_indi_dd.ClientID %>').checked == true) {

                var idval = document.getElementById("<%=div_indi_dd.ClientID %>");
                idval.style.display = "block";

                document.getElementById('<%=txt_chequename.ClientID %>').value = "";
                document.getElementById('<%=txt_chequenumb.ClientID %>').value = "";
                document.getElementById('<%=txt_chequeamt.ClientID %>').value = "";
                var idval1 = document.getElementById("<%=div_indi_cheque.ClientID %>");
                idval1.style.display = "none";
                return false;
            }
            else {
                var idval = document.getElementById("<%=div_indi_dd.ClientID %>");
                idval.style.display = "none";
                return false;
            }
        }
        function chequechangemag() {
            if (document.getElementById('<%=rdb_mag_chwque.ClientID %>').checked == true) {
                var idval = document.getElementById("<%=div_mag_cheque.ClientID %>");
                idval.style.display = "block";
                document.getElementById('<%=txt_ddname.ClientID %>').value = "";
                document.getElementById('<%=txt_ddno.ClientID %>').value = "";
                document.getElementById('<%=txt_ddamt.ClientID %>').value = "";
                var idval1 = document.getElementById("<%=div_mag_dd.ClientID %>");
                idval1.style.display = "none";
                return false;
            }
            else {
                var idval = document.getElementById("<%=div_mag_cheque.ClientID %>");
                idval.style.display = "none";
                return false;
            }
        }

        function ddchangemag() {
            if (document.getElementById('<%=rdb_mag_dd.ClientID %>').checked == true) {

                var idval = document.getElementById("<%=div_mag_dd.ClientID %>");
                idval.style.display = "block";

                document.getElementById('<%=txt_chequename.ClientID %>').value = "";
                document.getElementById('<%=txt_chequenumb.ClientID %>').value = "";
                document.getElementById('<%=txt_chequeamt.ClientID %>').value = "";
                var idval1 = document.getElementById("<%=div_mag_cheque.ClientID %>");
                idval1.style.display = "none";
                return false;
            }
            else {
                var idval = document.getElementById("<%=div_mag_dd.ClientID %>");
                idval.style.display = "none";
                return false;
            }
        }
        function chequechangeothr() {
            if (document.getElementById('<%=rdo_othr_cheque.ClientID %>').checked == true) {
                var idval = document.getElementById("<%=div_othr_cheque.ClientID %>");
                idval.style.display = "block";
                document.getElementById('<%=txt_ddname.ClientID %>').value = "";
                document.getElementById('<%=txt_ddno.ClientID %>').value = "";
                document.getElementById('<%=txt_ddamt.ClientID %>').value = "";
                var idval1 = document.getElementById("<%=div_othr_dd.ClientID %>");
                idval1.style.display = "none";
                return false;
            }
            else {
                var idval = document.getElementById("<%=div_othr_cheque.ClientID %>");
                idval.style.display = "none";
                return false;
            }
        }

        function ddchangeothr() {
            if (document.getElementById('<%=rdo_othr_dd.ClientID %>').checked == true) {

                var idval = document.getElementById("<%=div_othr_dd.ClientID %>");
                idval.style.display = "block";

                document.getElementById('<%=txt_chequename.ClientID %>').value = "";
                document.getElementById('<%=txt_chequenumb.ClientID %>').value = "";
                document.getElementById('<%=txt_chequeamt.ClientID %>').value = "";
                var idval1 = document.getElementById("<%=div_othr_cheque.ClientID %>");
                idval1.style.display = "none";
                return false;
            }
            else {
                var idval = document.getElementById("<%=div_othr_dd.ClientID %>");
                idval.style.display = "none";
                return false;
            }
        }
    </script>
    <form id="form1">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <center>
            <div>
                <center>
                    <div>
                        <span class="fontstyleheader" style="color:#008000;">Inward Exit / Entry</span></div>
                </center>
            </div><br />
            <div class="maindivstyle maindivstylesize">
                <br />
                <div>
                    <center>
                        <table class="table" width="900px">
                        
                            <tr>
                                <td id="td_stud" runat="server" align="center" >
                                    <asp:ImageButton ID="imgbtn_stud" runat="server" OnClientClick="return studentcheck();" OnClick="imgbtn_stud_Click"  
                                        Width="50px" Height="50px" Text="Student" ImageUrl="~/Hostel Gete Images/download_stud.jpg" />
                                    <br />
                                    <asp:Label ID="lbl_student" runat="server" Style="top: 10px; left: 6px;" Text="Student"></asp:Label>
                                     
                                </td>
                                <td id="td_staff" runat="server" align="center" onkeydown = "return (event.keyCode!=13);"> 
                                    <asp:ImageButton ID="imgbtn_staff" runat="server" Width="50px" Height="50px" ImageUrl="~/Hostel Gete Images/download_staff2.jpg"
                                        OnClientClick="return staffcheck();" OnClick="imgbtn_staff_Click" /><br />
                                    <asp:Label ID="lbl_staff" runat="server" Style="top: 10px; left: 6px;" Text="Staff"></asp:Label>
                                </td>
                                <td id="td_par" runat="server" align="center">
                                    <asp:ImageButton ID="imgbtn_parents" runat="server" Width="50px" Height="50px" ImageUrl="~/Hostel Gete Images/download_parents1.jpg"
                                        OnClientClick="return parentcheck();" OnClick="imgbtn_parents_Click" /><br />
                                    <asp:Label ID="lbl_parents" runat="server" Style="top: 10px; left: 6px;" Text="Parents"></asp:Label>
                                </td>
                                <td id="td_comp" runat="server" align="center">
                                    <asp:ImageButton ID="imgbtn_company" runat="server" Width="50px" Height="50px" ImageUrl="~/Hostel Gete Images/images_company2.jpg"
                                        OnClientClick="return companycheck();" OnClick="imgbtn_company_Click" /><br />
                                    <asp:Label ID="lbl_company" runat="server" Style="top: 10px; left: 6px;" Text="Company"></asp:Label>
                                </td>
                                <td id="td_indi" runat="server" align="center">
                                    <asp:ImageButton ID="imgbtn_individual" runat="server" Width="50px" Height="50px"
                                        ImageUrl="~/Hostel Gete Images/indivi.jpg" OnClientClick="return individualcheck();"
                                        OnClick="imgbtn_individual_Click" /><br />
                                    <asp:Label ID="lbl_individual" runat="server" Style="top: 10px; left: 6px;" Text="Institution"></asp:Label>
                                </td>
                                <td id="td_mag" runat="server" align="center">
                                    <asp:ImageButton ID="imgbtn_magazine" runat="server" Width="50px" Height="50px" ImageUrl="~/Hostel Gete Images/images -magazines.jpg"
                                        OnClientClick="return magazinecheck();" OnClick="imgbtn_magazine_Click" /><br />
                                    <asp:Label ID="lbl_magazine" runat="server" Style="top: 10px; left: 6px;" Text="Magazine"></asp:Label>
                                </td>
                                <td id="td_othr" runat="server" align="center">
                                    <asp:ImageButton ID="imgbtn_others" runat="server" Width="50px" Height="50px" ImageUrl="~/Hostel Gete Images/images (1)_other1.jpg"
                                        OnClientClick="return otherscheck();" OnClick="imgbtn_others_Click" /><br />
                                    <asp:Label ID="lbl_others" runat="server" Style="top: 10px; left: 6px;" Text="Others"></asp:Label>
                                </td>

                                  <td id="td_entryReport" runat="server" align="center">
                                    <asp:ImageButton ID="imgbtn_entryReport" runat="server" 
 Width="50px" Height="50px" ImageUrl="~/Hostel Gete Images/report.png" OnClientClick="return checkreportsave(this);"
                                        OnClick="imgbtn_entryReport_Click" /><br />
                                    <asp:Label ID="lbl_entryReport" runat="server" Style="top: 10px; left: 6px;" Text="Entry Report"></asp:Label>
                                </td>
                               
                            </tr>
                          
                          <tr>
                           <td>
                                                <asp:Label ID="lbl_fromdate" Text="From Date"  runat="server" style="float: left; margin-left:33px;"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="Updp_fromdate" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txt_fromdate" runat="server" CssClass="textbox txtheight2" OnTextChanged="txt_fromdate_TextChanged"
                                                            AutoPostBack="true"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender22" TargetControlID="txt_fromdate" runat="server"
                                                            CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>

                                            
 <td>
                                                <asp:Label ID="lbl_todate" Text="To Date" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txt_todate" runat="server" CssClass="textbox txtheight2" OnTextChanged="txt_todate_TextChanged"
                                                            AutoPostBack="true"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender23" TargetControlID="txt_todate" runat="server"
                                                            CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                          
                          
                          </tr>
                        </table>
                         <br />
                          <br />
                        
                    </center>
                </div>
              <br />
                <div id="spread" runat="server">
                        <table>
                        <tr>
                        <td id="spreadid">
                         <FarPoint:FpSpread ID="Fpspread9" runat="server" Visible="false" BorderWidth="4px"
                                          BorderStyle="Groove" BorderColor="#0CA6CA" ActiveSheetViewIndex="0" >
                                            <Sheets>
                                                <FarPoint:SheetView SheetName="Sheet1">
                                                </FarPoint:SheetView>
                                            </Sheets>
                                        </FarPoint:FpSpread> <%--OnButtonCommand="fpspread9_ButtonCommand"--%>
                                        </td>
                                        </tr>
                                        </table>
               
                                        </div>
                                        <%--Added By saranya on 20Aug2018--%>
            <center>
                        <div id="print" runat="server" visible="false">
                            <asp:Label ID="lblvalidation1" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                ForeColor="Red" Text="" Visible="false"></asp:Label>
                            <asp:Label ID="lblrptname" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                Text="Report Name"></asp:Label>
                            <asp:TextBox ID="txtexcelname" runat="server" Width="180px" onkeypress="display(this)"
                                CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender172" runat="server" TargetControlID="txtexcelname"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                InvalidChars="/\">
                            </asp:FilteredTextBoxExtender>
                            <asp:Button ID="btnExcel" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                OnClick="btnExcel_Click" Text="Export To Excel" Width="127px" Height="32px" CssClass="textbox textbox1" />
                            <asp:Button ID="btnprintmasterhed" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                Text="Print" OnClick="btnprintmaster_Click" Height="32px" Style="margin-top: 10px;"
                                CssClass="textbox textbox1" Width="60px" />
                            <Insproplus:printmaster runat="server" ID="Printcontrolhed" Visible="false" />
                        </div>
                    </center>
             

              <div id="div_college" runat="server" style="display: none">
              <center><table class="maindivstyle"><tr><td><asp:Label ID="lbl_college" runat="server" Text="College"></asp:Label></td>
              <td>
              <asp:DropDownList ID="ddl_college" runat="server" CssClass="ddlheight6 textbox textbox1"></asp:DropDownList>
              </td>
              &nbsp&nbsp  &nbsp&nbsp
                                <td>
                                    <asp:TextBox ID="txt_rcptno" runat="server" CssClass="textbox txtheight" Height="15px"
                                        Style="text-align: center;" Enabled="false" BackColor="#81F7D8"></asp:TextBox>
                                    <asp:Label ID="lblstaticrollno" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="lblaccid" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="lstrcpt" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="lbltype" runat="server" Visible="false"></asp:Label>
                                </td>
              </tr></table></center>
              </div>
                <div id="div_stud" runat="server" style="display: none">
                    <br />
                    <center><span Style="font-size: x-large; color: #008000;">Student</span></center>
                     <center>
                        <table width="900px">
                            <tr>
                           
                                <td align="right">
                                 <div class="maindivstyle" align="center" style="border-radius: 7px; width: 120px;
                                    height: 30px;">
                               <%-- <td>--%>
                                    <asp:RadioButton ID="rb_in" runat="server" RepeatDirection="Horizontal" GroupName="same11"
                                        Text="In" Checked="true" onchange="return save_stud_clear1();"></asp:RadioButton>
                               <%-- </td>
                                <td>--%>
                                    <asp:RadioButton ID="rb_out" runat="server" RepeatDirection="Horizontal" GroupName="same11"
                                        Text="Out" onchange="return save_stud_clear();"></asp:RadioButton>
                                        </div>
                               <%-- </td>--%>
                                </td>
                                
                                <td style="width:100">
                                
                                </td>
                                <td style="width:250px"> </td>
                                
                       <td style="width:200px"></td>
                           
                               
                                <td align="right">
                                  <div class="maindivstyle" align="center" style="border-radius: 7px; width: 380px;
                                    height: 40px;">
                                    <table>
                                    <tr>
                                    <td>
                                    <asp:Label ID="lbl_date" runat="server" Style="top: 10px; left: 6px;" Text="Date"></asp:Label>
                               </td>
                               <td>
                                 <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_date" runat="server" CssClass="textbox txtheight textbox1" OnTextChanged="txt_datestud_changed" AutoPostBack="true">
                                                </asp:TextBox>
                                                 <asp:CalendarExtender ID="Cal_date" TargetControlID="txt_date" runat="server"
                                                CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            </ContentTemplate>
                                            </asp:UpdatePanel>
                                  
                                </td>
                                <td>
                                    <asp:Label ID="lbl_time" runat="server" Style="top: 10px; left: 6px;" Text="Time"></asp:Label>
                              </td>
                              <td>
                                     
                                    <asp:DropDownList ID="ddl_hour" Width="50px" Height="25px" runat="server"  CssClass="textbox textbox1">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddl_minits" Width="50px" Height="25px" runat="server" CssClass="textbox textbox1">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddl_seconds" Width="50px" Height="25px" Visible="false" runat="server"
                                        CssClass="textbox textbox1">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddl_timeformate" Width="50px" Height="25px" runat="server"
                                        CssClass="textbox textbox1">
                                        <asp:ListItem>AM</asp:ListItem>
                                        <asp:ListItem>PM</asp:ListItem>
                                    </asp:DropDownList>
                               
                                        </td>
                                        </tr>
                                        </table>
                                        </div>
                                </td>
                            </tr>
                        </table>
                     </center>
                     <br />
                  
                    <div style="width: 900px">
                        <div id="cheiddiv" style="float: left">
                            <table>
                                <tr>
                                    <td colspan="2" align="center">
                                     <asp:Label ID="lbl_chn_to" ForeColor="#008000" Font-Bold="true" Text="To" style="display:none;" onfocus="return myFunction(this)" runat="server"></asp:Label>
                                        <asp:Label ID="lbl_from" ForeColor="#008000" Font-Bold="true" Text="From" runat="server" onfocus="return myFunction(this)"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:RadioButton ID="rdo_current" Text="Current Student"   runat="server" GroupName="same" onchange="return rbcurrentold();"  />
                                        <asp:RadioButton ID="rdo_old" Text="Old Student" runat="server"   GroupName="same" onchange="return rbcurrentold1();" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_roll" Text="Roll No" runat="server" onfocus="return myFunction(this)"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_roll" runat="server" CssClass="textbox txtheight4 textbox1" onkeydown = "return (event.keyCode!=13);" onchange="return checkrno(this.value)" onkeyup="return checkrno(this.value)"
                                            onblur="return get(this.value)" onfocus="return myFunction(this)" OnTextChanged="txt_roll_Ontextchanged"></asp:TextBox> 
                                                                       
                                        <asp:FilteredTextBoxExtender ID="ftext_rollno" runat="server" TargetControlID="txt_roll"
                                            FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:AutoCompleteExtender ID="acext_rollno" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_roll"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender><%--barath style="display: none"--%>
                                      
                                         <span style="color: Red;">*</span>
                                        <span id="rnomsg"></span>

                                        <asp:Button ID="enqbtn" runat="server" Text="?" Height="20px" OnClick="enqbtn_Click"
                                            Width="20px" />

                                    </td>
                                </tr>
                                <tr>
                                <td><%--barath style="display: none"--%>
                                     <asp:Label ID="lbl_reg" Text="Register No" runat="server" onfocus="return myFunction(this)"></asp:Label> </td>
                                     <td>    <asp:TextBox ID="txt_reg" runat="server"  CssClass="textbox txtheight4 textbox1" onkeydown = "return (event.keyCode!=13);" onchange="return checkregno(this.value)" onkeyup="return checkregno(this.value)"
                                            onblur="return getreg(this.value)" onfocus="return myFunction(this)"></asp:TextBox> 
                                              <asp:AutoCompleteExtender ID="AutoCompleteExtender18" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getreg" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_reg"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>   
                                         <span style="color: Red;">*</span>
                                         <span id="regmsg"></span>
                                         </td>
                                
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_name" Text="Name" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_name" runat="server" CssClass="textbox txtheight6 textbox1" onkeydown = "return (event.keyCode!=13);"
                                            onblur="getname(this.value)"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="ftext_name" runat="server" TargetControlID="txt_name"
                                            FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:AutoCompleteExtender ID="acext_name" runat="server" DelimiterCharacters="" Enabled="True"
                                            ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                                            CompletionSetCount="10" ServicePath="" TargetControlID="txt_name" CompletionListCssClass="autocomplete_completionListElement"
                                            CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_batch" Text="Batch year" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_batch" runat="server" CssClass="textbox txtheight1" Enabled="false" BackColor="#ffffcc"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_degree" Text="Degree" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_degree" runat="server" CssClass="textbox txtheight1" Enabled="false" BackColor="#ffffcc"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_department" Text="Department" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_department" runat="server" CssClass="textbox txtheight6" Enabled="false" BackColor="#ffffcc"></asp:TextBox>
                                    </td>
                                </tr>
                                 <tr>
                                    <td>
                                        <asp:Label ID="lbl_section" Text="Section" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_section" runat="server" CssClass="textbox txtheight1" Enabled="false" BackColor="#ffffcc"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_sem" Text="Semester" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_sem" runat="server" CssClass="textbox txtheight1" Enabled="false" BackColor="#ffffcc"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_studtype" Text="Student Type" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_studtype" runat="server" CssClass="textbox txtheight1" Enabled="false" BackColor="#ffffcc"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_addrs" Text="Address" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_addrs" runat="server" CssClass="textbox txtheight5" Enabled="false" BackColor="#ffffcc"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_street" Text="Street" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_street" runat="server" CssClass="textbox txtheight4" Enabled="false" BackColor="#ffffcc"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_city" Text="City" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_city" runat="server" CssClass="textbox txtheight4" Enabled="false" BackColor="#ffffcc"></asp:TextBox>
                                    </td>
                                </tr>
                                  <tr>
                                    <td>
                                        <asp:Label ID="lbl_pincode_stud" Text="Pin Code" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_pin_stud" runat="server" CssClass="textbox txtheight4" Enabled="false" BackColor="#ffffcc"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_state" Text="State" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_state" runat="server" CssClass="textbox txtheight4" Enabled="false" BackColor="#ffffcc"></asp:TextBox>
                                    </td>
                                </tr>
                                 <tr>
                                    <td>
                                        <asp:Label ID="lbl_stud_country" Text="Country" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_stud_country" runat="server" CssClass="textbox txtheight4" Enabled="false" BackColor="#ffffcc"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_phone" Text="Phone No" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_pho" runat="server" CssClass="textbox txtheight4" Enabled="false" BackColor="#ffffcc"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_mailid" Text="Mail ID" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_mailid" runat="server" CssClass="textboxchng txtheight5" Enabled="false" BackColor="#ffffcc"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div style="float: left; margin-left: 10px;">
                            <table>
                                <tr>
                                    <td colspan="2" align="center">
                                    <asp:Label ID="lbl_chng_frm" ForeColor="#008000" Font-Bold="true" Text="From" runat="server" style="display:none;" 
onfocus="return myFunction(this)" ></asp:Label>
                                        <asp:Label ID="lbl_to" ForeColor="#008000" Font-Bold="true" Text="To" runat="server" 
onfocus="return myFunction(this)"></asp:Label>
                                    </td>
                                    <br />
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:CheckBox ID="cb_dept" runat="server" Checked="true" Text="Department" OnCheckedChanged="cb_dept_CheckedChanged"
                                            onchange="return checkchange1(this.value)" onfocus="return myFunction(this)" />
                                        <asp:CheckBox ID="cb_individual" runat="server" Text="Individual" onchange="checkchange2(this)"
                                            onfocus="return myFunction(this)" />
                                    </td>
                                </tr>
                            </table>
                            <div id="div_dept" runat="server" style="display: none" onfocus="return myFunction(this)">
                            <span id="deptmsg"></span>
                             <span id="indimsg"></span>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_dept_to" Text="Department" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                       
                                            <asp:TextBox ID="txt_dept_to" runat="server" Width="210px" Height="20px"  CssClass="textbox1 textbox"  onkeydown = "return (event.keyCode!=13);" onfocus="return myFunction(this)" onchange="return checkdepartment(this.value)" onkeyup="return checkdepartment(this.value)"></asp:TextBox>
                                        
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_dept_to"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="auto_dept" runat="server" DelimiterCharacters="" Enabled="True"
                                                ServiceMethod="Getdept" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                                                CompletionSetCount="10" ServicePath="" TargetControlID="txt_dept_to" CompletionListCssClass="autocomplete_completionListElement"
                                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                             <asp:Button ID="Button1" runat="server" Text="Add" CssClass="textbox btn1 textbox1"
                                                OnClientClick="return change3();" />
                                                 <asp:Button ID="btn_stud_deptto_rmv" runat="server" Width="58px" Text="Remove" CssClass="textbox btn1 textbox1"
                                                OnClientClick="return change31();" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_to1" runat="server" Style="display: none" onkeydown = "return (event.keyCode!=13);" onchange="return checkdepartment(this.value)" onkeyup="return checkdepartment(this.value)" onfocus="return myFunction(this)"
                                                Width="210px" Height="20px" CssClass="textbox1 textbox"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_to1"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="auto_dept1" runat="server" DelimiterCharacters="" Enabled="True"
                                                ServiceMethod="Getdept" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                                                CompletionSetCount="10" ServicePath="" TargetControlID="txt_to1" CompletionListCssClass="autocomplete_completionListElement"
                                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_dept_cc" Text="Cc" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_dept_cc" runat="server" Width="210px" onkeydown = "return (event.keyCode!=13);" onchange="return checkdepartment(this.value)" onkeyup="return checkdepartment(this.value)" onfocus="return myFunction(this)" Height="20px" CssClass="textbox1 textbox"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_dept_cc"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="auto_dept2" runat="server" DelimiterCharacters="" Enabled="True"
                                                ServiceMethod="Getdept" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                                                CompletionSetCount="10" ServicePath="" TargetControlID="txt_dept_cc" CompletionListCssClass="autocomplete_completionListElement"
                                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <asp:Button ID="btnadd3" runat="server" CssClass="textbox btn1 textbox1" Text="Add"
                                                OnClientClick="return change4();" OnClick="btn_add3_Click" />
                                                <asp:Button ID="btn_stud_deptcc_remove" runat="server" Width="58px" CssClass="textbox btn1 textbox1" Text="Remove"
                                                OnClientClick="return change41();" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_cc1" runat="server" onchange="return checkdepartment(this.value)" onkeyup="return checkdepartment(this.value)" Style="display: none" onfocus="return myFunction(this)"
                                                Width="210px" Height="20px" CssClass="textbox1 textbox"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txt_cc1"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="auto_dept3" runat="server" DelimiterCharacters="" Enabled="True"
                                                ServiceMethod="Getdept" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                                                CompletionSetCount="10" ServicePath="" TargetControlID="txt_cc1" CompletionListCssClass="autocomplete_completionListElement"
                                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="div_indiv" runat="server" style="display: none; margin-left:11px" onfocus="return myFunction(this)">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_indiv" Text="Individual" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_indiv" runat="server" Width="210px" onchange="return checkindiv(this.value)" onkeyup="return checkindiv(this.value)"  onfocus="return myFunction(this)" Height="20px" CssClass="textbox1 textbox"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender55" runat="server" TargetControlID="txt_indiv"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom,Numbers" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="autostudindi1" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getstaff" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_indiv"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <asp:Button ID="Button2" runat="server" CssClass="textbox btn1 textbox1" Text="Add"
                                                OnClientClick="return change5();" OnClick="btn_add1_Click" />
                                                 <asp:Button ID="btn_stud_indito_rmv" Width="58px" runat="server" CssClass="textbox btn1 textbox1" Text="Remove"
                                                OnClientClick="return change51();" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_indiv1" runat="server" onchange="return checkindiv(this.value)" onkeyup="return checkindiv(this.value)" Style="display: none" onfocus="return myFunction(this)"
                                                Width="210px" Height="20px" CssClass="textbox1 textbox"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txt_indiv1"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom,Numbers" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="autostudindi2" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getstaff" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_indiv1"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_indiv_cc" Text="Cc" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_indiv_cc" runat="server" Width="210px" onchange="return checkindiv(this.value)" onkeyup="return checkindiv(this.value)"  onfocus="return myFunction(this)" Height="20px" CssClass="textbox1 textbox"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender57" runat="server" TargetControlID="txt_indiv_cc"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom,Numbers" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="autostudindi3" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getstaff" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_indiv_cc"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <asp:Button ID="btnadd2" runat="server" CssClass="textbox btn1 textbox1" Text="Add"
                                                OnClientClick="return change6();" OnClick="btn_add2_Click" />
                                                <asp:Button ID="btn_stud_indicc_rmv" runat="server" Width="58px" CssClass="textbox btn1 textbox1" Text="Remove"
                                                OnClientClick="return change61();"  />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_cc2" runat="server" onchange="return checkindiv(this.value)" onkeyup="return checkindiv(this.value)" Style="display: none" onfocus="return myFunction(this)"
                                                Width="210px" Height="20px" CssClass="textbox1 textbox"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender58" runat="server" TargetControlID="txt_cc2"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom,Numbers" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="autostudindi4" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getstaff" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_cc2"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                    <div style="float: left; margin-left: 140px;">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_stud" Text="Subject" runat="server"></asp:Label>  <%--delsi1901--%>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_stud_sub" runat="server" onfocus="return myFunction(this)" MaxLength="100" Visible="true"  Width="700px"  CssClass="textboxchng textbox1 txtheight6" onkeydown = "return (event.keyCode!=13);"> </asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender162" runat="server" TargetControlID="txt_stud_sub"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom,Numbers" ValidChars=" .-&@/*">

                                            </asp:FilteredTextBoxExtender>
                                         <span style="color: Red;">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_stud_doc" Text="Document Priority" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_stud_doc" runat="server" CssClass="textbox  ddlheight4"
                                        Style="float: left;" onchange="changecompanyddl(this)" onfocus="return myFunction(this)">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txt_stud_doc_others" runat="server" CssClass="textbox txtheight4 textbox1"
                                        Style="display: none; float: left;" onfocus="return myFunction(this)" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender163" runat="server" TargetControlID="txt_stud_doc_others"
                                                FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                              <tr>
                                <td>
                                    <asp:Label ID="lbl_stud_delivery" Text="Received Mode" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_stud_delivery" runat="server" CssClass="textbox  ddlheight4"
                                        Style="float: left;" onchange="changestud(this)" onfocus="return myFunction(this)">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txt_stud_delivery" runat="server" CssClass="textbox txtheight4 textbox1"
                                        Style="display: none; float: left;" onfocus="return myFunction(this)"></asp:TextBox>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <asp:Label ID="lbl_stud_ref" Text="Ref No" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_stud_ref" runat="server" CssClass="textbox txtheight1 textbox1" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender164" runat="server" TargetControlID="txt_stud_ref"
                                                 FilterType="Custom,UppercaseLetters,LowercaseLetters,numbers" ValidChars=" .#$*&/-@">
                                            </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>

                             <tr>
                                <td>
                                    <asp:Label ID="lb1_stud_attach" Text="Attachments" runat="server"></asp:Label>
                                </td>
                               
                                        <td align="left">
                                
                    
                                <div id="studgrid" runat="server" >
                                <center>
                                  <span style="color: White; font-weight: bold;"></span>
                                  </center>
                                            <asp:GridView ID="grid_stude_attach" runat="server" AutoGenerateColumns="false" Visible="true">
                                                <Columns>
                                                                                            
                                                    <asp:TemplateField HeaderText="No of Files" HeaderStyle-ForeColor="White"  HeaderStyle-BackColor="Brown"  HeaderStyle-HorizontalAlign="center">
                                                        <ItemTemplate>
                                                          
                                                            <asp:Label ID="lbl_stud_upload" ReadOnly="true" runat="server" Text='<%#Eval("Files") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Browse"  HeaderStyle-ForeColor="White" HeaderStyle-BackColor="Brown" HeaderStyle-HorizontalAlign="center">
                                                        <ItemTemplate>
                                                        <asp:Label ID="lbl_mandatory_stud" runat="server" Text="*"></asp:Label>
                                                            <asp:FileUpload ID="file_uplodeStud" runat="server" />
                                                            
                                                        </ItemTemplate>
                                                       
                                                    </asp:TemplateField>

                                                </Columns>
                                            </asp:GridView>
                                        </div> 

                                         
                                      <asp:Button ID="btn_studGrid" runat="server" CssClass="textbox textbox1 type" BackColor="Brown"
                                    ForeColor="White"  Visible="true" Width="81px" Height="26px" Text="Add Rows"
                                     OnClick="btnaddrows1_studClick" />
                                 
                                         </td>
                                     
                            </tr>
                             <tr>
                            <td>
                           
                         <asp:Label ID="lbl_stud_charges" runat="server" Text="Charges"></asp:Label></td>
                         <td>
                         <asp:TextBox ID="txt_stud_charges" runat="server" CssClass="textbox textbox1 txtheight1" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                          <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender165" runat="server" TargetControlID="txt_stud_charges"
                                            FilterType="numbers" ValidChars="">
                                        </asp:FilteredTextBoxExtender></td>
                            </tr>
                             <tr>
                                        <td>
                                        <asp:Label ID="lbl_stud_charg" Text="Payment" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                        <asp:CheckBox ID="chk_stud_charges" runat="server" onchange="return studpayment1(this.value)" onfocus="return myFunction(this)"  /></td>
                            </tr>

                            </table>
                            

                            <div id="div_student_payment" runat="server" style="display:none" class="maindivstyle">
                             <table ><tr>
                            <td>
                          
                            <asp:RadioButton ID="rdo_stud_cheque" runat="server" Checked="true" GroupName="pay3" Text="Cheque" onchange="return chequechangestud(this.value)" onfocus="return myFunction(this)"  />
                           
                            <asp:RadioButton ID="rdo_stud_chedd" runat="server" GroupName="pay3" Text="DD" onchange="return ddchangestud(this.value)" onfocus="return myFunction(this)"  />
                            <asp:Label ID="lbl_student_bank" Text="Bank" runat="server"></asp:Label>
                            </td>
                            <td>
                           

                            <asp:DropDownList ID="ddl_bank_stud" runat="server" CssClass="textbox  ddlheight4"
                                         onchange="changebankstud(this)" onfocus="return myFunction(this)"></asp:DropDownList>
                                         </td>
                                         <td>
                                         
                            <asp:TextBox ID="txt_bank_student" runat="server" CssClass="textbox txtheight4 textbox1" Style="display: none;
                                        " onfocus="return myFunction(this)"></asp:TextBox>
                                        </td>
                                        <td>
                                
                            <asp:Label ID="lbl_student_bankbranch" Text="Branch" runat="server"></asp:Label>
                            <asp:TextBox ID="txt_stud_bankbranch" runat="server" CssClass="textbox textbox1 txtheight1" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                          
                         
                        </td></tr></table>
                         </div>
                        <br />
                            <div id="div_stud_cheque" runat="server" style="display:none" class="maindivstyle">
                            <table>
                            <tr>
                            <td><asp:Label ID="lbl_stud_chqname" runat="server" Text="Name"></asp:Label></td>
                            <td><asp:TextBox ID="txt_stud_chqname" runat="server" CssClass="txtheight4 textbox textbox1"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender166" runat="server" TargetControlID="txt_stud_chqname"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender></td>
                             <td><asp:Label ID="lbl_stud_chqno" runat="server" Text="Cheque No"></asp:Label></td>
                            <td><asp:TextBox ID="txt_stud_chqno" runat="server" CssClass="txtheight textbox textbox1"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender167" runat="server" TargetControlID="txt_stud_chqno"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom,numbers" ValidChars=" ">
                                </asp:FilteredTextBoxExtender></td>
                             <td><asp:Label ID="lbl_stud_chqdate" runat="server" Text="Date"></asp:Label></td>
                            <td><asp:TextBox ID="txt_stud_chqdate" runat="server" CssClass="txtheight textbox textbox1"></asp:TextBox>
                             <asp:CalendarExtender ID="CalendarExtender20" TargetControlID="txt_stud_chqdate" runat="server"
                                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender></td>
                             <td><asp:Label ID="lbl_stud_chqamt" runat="server" Text="Amount"></asp:Label></td>
                            <td><asp:TextBox ID="txt_stud_chqamt" runat="server" CssClass="txtheight2 textbox textbox1"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender168" runat="server" TargetControlID="txt_stud_chqamt"
                                    FilterType="Numbers" ValidChars="">
                                </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            </table>
                            </div>
                              <div id="div_student_dd" runat="server" style="display:none" class="maindivstyle">
                            <table>
                            <tr>
                            <td><asp:Label ID="lbl_studddname" runat="server" Text="Name"></asp:Label></td>
                            <td><asp:TextBox ID="txt_studddname" runat="server" CssClass="txtheight4 textbox textbox1"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender169" runat="server" TargetControlID="txt_studddname"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender></td>
                             <td><asp:Label ID="lbl_studddno" runat="server" Text="DD No"></asp:Label></td>
                            <td><asp:TextBox ID="txt_studddno" runat="server" CssClass="txtheight textbox textbox1"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender170" runat="server" TargetControlID="txt_studddno"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom,numbers" ValidChars=" ">
                                </asp:FilteredTextBoxExtender></td>
                             <td><asp:Label ID="lbl_stud_dddate" runat="server" Text="Date"></asp:Label></td>
                            <td><asp:TextBox ID="txt_stud_dddate" runat="server" CssClass="txtheight textbox textbox1"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender21" TargetControlID="txt_stud_dddate" runat="server"
                                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender></td>
                             <td><asp:Label ID="lbl_stud_ddamt" runat="server" Text="Amount"></asp:Label></td>
                            <td><asp:TextBox ID="txt_stud_ddamt" runat="server" CssClass="txtheight2 textbox textbox1"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender171" runat="server" TargetControlID="txt_stud_ddamt"
                                    FilterType="Numbers" ValidChars="">
                                </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            </table>
                            </div>
                    </div>


                    <div style="float: left; margin-left: 95px;display: none;">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_subject" Text="Subject" runat="server"></asp:Label>
                                </td>
                                <td colspan="4" style="width:700px">
                                    <asp:TextBox ID="txt_subject" runat="server" CssClass="textboxchng txtheight6 textbox1"  onfocus="return myFunction(this)" onkeyup="myFunCaps(this.id)" Width="650px"></asp:TextBox>
                                     <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender72" runat="server" TargetControlID="txt_subject"
                                            FilterType="LowercaseLetters,UppercaseLetters,custom,Numbers" ValidChars=" ._">
                                        </asp:FilteredTextBoxExtender>
                                         <span style="color: Red;">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_docpriv" Text="Document Priority" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_docpriv" runat="server" CssClass="textbox ddlheight5 textbox1"
                                        Style="float: left;" onchange="change1(this)" onfocus="return myFunction(this)">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txt_docpriv" CssClass="textbox txtheight4 textbox1" Style="display: none;
                                        float: left;" onfocus="return myFunction(this)" runat="server"></asp:TextBox>
                                         <asp:FilteredTextBoxExtender ID="flt_docpiv" runat="server" TargetControlID="txt_docpriv"
                                            FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                        </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_delivery" Text="Delivery Mode" runat="server" Style="float: left;"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_delivery" runat="server" CssClass="textbox ddlheight5 textbox1"
                                        Style="float: left;" onchange="change2(this)" onfocus="return myFunction(this)">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txt_delivery" CssClass="textbox txtheight4 textbox1" Style="display: none;
                                        float: left;" onfocus="return myFunction(this)" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_refno" Text="Ref No" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="ttx_refno" runat="server" CssClass="textbox txtheight4 textbox1"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender75" runat="server" TargetControlID="ttx_refno"
                                            FilterType="Custom,UppercaseLetters,LowercaseLetters,numbers" ValidChars=" .#$*&">
                                        </asp:FilteredTextBoxExtender>
                                </td>
                               
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_attach" Text="Attachments" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:FileUpload ID="fileupload" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" />
                                    <asp:Button ID="btn_more" runat="server" CssClass="textbox textbox1 btn2" Text="More" OnClientClick="return browse_change();"
                                        OnClick="btn_more_Click" Visible="false" />
                                </td>
                            </tr>
                            <tr>
                           <td>
                         <asp:Label ID="lbl_chargestud" runat="server" Text="Charges"></asp:Label></td>
                         <td>
                         <asp:TextBox ID="txt_chargestud" runat="server" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                          <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender114" runat="server" TargetControlID="txt_chargestud"
                                            FilterType="numbers" ValidChars="">
                                        </asp:FilteredTextBoxExtender>
                                        </td>
                                        </tr>
                                        <tr>
                                        <td>
                                        <asp:Label ID="lbl_paymentstud" Text="Payment" runat="server"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                        <asp:CheckBox ID="cb_paymentstud" runat="server" onchange="return studpayment(this.value)" onfocus="return myFunction(this)"  />
                             </td>
                          
                           </tr>
                             </table>
                             <table ><tr>
                            <td>
                           <div id="div_stud_payment" runat="server" style="display:none" class="maindivstyle">
                          
                            <asp:RadioButton ID="rdo_cheque" runat="server" Checked="true" GroupName="pay" Text="Cheque" onchange="return chequechange(this.value)" onfocus="return myFunction(this)"  />
                           
                            <asp:RadioButton ID="rdo_dd" runat="server" GroupName="pay" Text="DD" onchange="return ddchange(this.value)" onfocus="return myFunction(this)"  />
                            <asp:Label ID="lbl_stud_bank" Text="Bank" runat="server"></asp:Label>
                            <asp:DropDownList ID="ddl_stud_bank" runat="server" CssClass="ddlheight1 textbox textbox1"></asp:DropDownList>
                            <asp:Label ID="lbl_stud_bankbranch" Text="Branch" runat="server"></asp:Label>
                            <asp:TextBox ID="txt_bankbranch" runat="server" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                           </div>
                         
                        </td></tr></table>
                        <br />
                            <div id="div_cheque" runat="server" style="display:none" class="maindivstyle">
                            <table>
                            <tr>
                            <td><asp:Label ID="lbl_chequename" runat="server" Text="Name"></asp:Label></td>
                            <td><asp:TextBox ID="txt_chequename" runat="server" CssClass="txtheight4 textbox textbox1"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="ftext_sersugvendor" runat="server" TargetControlID="txt_chequename"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender></td>
                             <td><asp:Label ID="lbl_chequenumb" runat="server" Text="Cheque No"></asp:Label></td>
                            <td><asp:TextBox ID="txt_chequenumb" runat="server" CssClass="txtheight textbox textbox1"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender124" runat="server" TargetControlID="txt_chequenumb"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom,numbers" ValidChars=" ">
                                </asp:FilteredTextBoxExtender></td>
                             <td><asp:Label ID="lbl_chequedate" runat="server" Text="Date"></asp:Label></td>
                            <td><asp:TextBox ID="txt_chequedate" runat="server" CssClass="txtheight textbox textbox1"></asp:TextBox>
                             <asp:CalendarExtender ID="Cal_date1" TargetControlID="txt_chequedate" runat="server"
                                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender></td>
                             <td><asp:Label ID="lbl_chqueamt" runat="server" Text="Amount"></asp:Label></td>
                            <td><asp:TextBox ID="txt_chequeamt" runat="server" CssClass="txtheight2 textbox textbox1"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender122" runat="server" TargetControlID="txt_chequeamt"
                                    FilterType="Numbers" ValidChars="">
                                </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            </table>
                            </div>
                              <div id="div_dd" runat="server" style="display:none" class="maindivstyle">
                            <table>
                            <tr>
                            <td><asp:Label ID="lbl_ddname" runat="server" Text="Name"></asp:Label></td>
                            <td><asp:TextBox ID="txt_ddname" runat="server" CssClass="txtheight4 textbox textbox1"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender121" runat="server" TargetControlID="txt_ddname"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender></td>
                             <td><asp:Label ID="lbl_ddno" runat="server" Text="DD No"></asp:Label></td>
                            <td><asp:TextBox ID="txt_ddno" runat="server" CssClass="txtheight textbox textbox1"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender125" runat="server" TargetControlID="txt_ddno"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom,numbers" ValidChars=" ">
                                </asp:FilteredTextBoxExtender></td>
                             <td><asp:Label ID="lbl_dddate" runat="server" Text="Date"></asp:Label></td>
                            <td><asp:TextBox ID="txt_dddate" runat="server" CssClass="txtheight textbox textbox1"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender7" TargetControlID="txt_dddate" runat="server"
                                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender></td>
                             <td><asp:Label ID="lbl_ddamt" runat="server" Text="Amount"></asp:Label></td>
                            <td><asp:TextBox ID="txt_ddamt" runat="server" CssClass="txtheight2 textbox textbox1"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender123" runat="server" TargetControlID="txt_ddamt"
                                    FilterType="Numbers" ValidChars="">
                                </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            </table>
                            </div>
                           
                          <asp:Label ID="lbl_er" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                    <br />
                  
                    <br />
                    <div id="div1" runat="server" style="float: left; margin-top:31px; margin-left: 400px" onfocus="return myFunction(this)">
                        <asp:Button ID="Button3" runat="server" CssClass="textbox textbox1 btn2" Text="Save" OnClientClick="return validstud();" OnClick="btn_save_Click"  />
                        <asp:Button ID="Button4" runat="server" CssClass="textbox textbox1 btn2" Text="Clear" OnClientClick="return div_stud_clear();"
                            OnClick="btn_clear_Click" /></div>
                </div>
                <%-- ---end of studnt------%>
                <%-- ----------popup1--------%>
                <center>
                 <asp:Panel runat="server" ID="Panellookup1" Visible="false" BackColor="AliceBlue"
                        Style="border: thin solid Black; left: 23px; top: 185px; width: 978px; height: 562px;
                        position: absolute;">
                         <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                            Style="height: 30px; width: 30px; position: absolute; margin-top: 0px; margin-left: 468px;"
                            OnClick="btncloselook1_Click" />

                        <center>
                            <asp:Label ID="Label25" runat="server" Text="Student LookUp" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </center>
                         <table style="width: 400px; height: 85px;">
                         <tr>
                          <td>
                                    <asp:Label ID="lblcollege1" runat="server" Text="College_Name" Font-Bold="true" Font-Names="MS Sans Serif"
                                        Font-Size="Small"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlcollegenew" runat="server" OnSelectedIndexChanged="ddlcollegenew_SelectedIndexChanged"
                                        Font-Names="MS Sans Serif" Font-Size="Small" Height="20px" Width="251px" Font-Bold="true"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                                 <td>
                                    <asp:Label ID="lblbatch" runat="server" Text="Batch" Font-Bold="true" Font-Names="MS Sans Serif"
                                        Font-Size="Small"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlbatch" runat="server" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged"
                                        Font-Names="MS Sans Serif" Font-Size="Small" Height="20px" Width="70px" Font-Bold="true"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbldegree" runat="server" Text="Degree" Font-Bold="true" Font-Names="MS Sans Serif"
                                        Font-Size="Small"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlDegree" runat="server" Font-Bold="true" Font-Names="MS Sans Serif"
                                        Font-Size="Small" Height="20px" Width="70px" AutoPostBack="True" OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                 <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:Label ID="lblbranch" runat="server" Text="Branch" Font-Bold="true" Font-Names="MS Sans Serif"
                                        Font-Size="Small"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlBranch1" runat="server" Font-Bold="true" Font-Names="MS Sans Serif"
                                        Font-Size="Small" Height="20px" Width="185px" OnSelectedIndexChanged="ddlBranch1_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                               
                                <td>
                                    <asp:Button ID="btnlookupgo1" runat="server" Text="Go" Height="21px" Style="top: 53px;
                                        position: absolute; left: 913px;" CssClass="font" OnClick="btnlookupgo1_Click" />
                                </td>
                         </tr>
                         </table>
                         <table style="width: 462px; height: 25px; top: 81px; left:45px; position: absolute;">
                         <tr>
                         <td>
                         </td>

                         <td>
                                    <asp:Label ID="Label87" runat="server" Text="Search By" Font-Names="MS Sans Serif"
                                        Font-Size="Small" Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlheader" runat="server" AutoPostBack="true" Width="100px"
                                        OnSelectedIndexChanged="ddlheader_SelectedIndexChanged" Font-Names="MS Sans Serif"
                                        Font-Size="Small">
                                    </asp:DropDownList>
                                </td>

                                <td>
                                    <asp:DropDownList ID="ddloperator" runat="server" AutoPostBack="true" Width="100px"
                                        OnSelectedIndexChanged="ddloperator_SelectedIndexChanged" Font-Names="MS Sans Serif"
                                        Font-Size="Small" Enabled="False">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbvalue" runat="server" AutoPostBack="true" OnTextChanged="tbvalue_TextChanged"
                                        Font-Names="MS Sans Serif" Font-Size="Small" Height="15px" Width="153px" Enabled="False"></asp:TextBox>
                                </td>
                         
                         </tr>
                         </table>
                          <asp:Label ID="lblerrefp1" runat="server" Text="" Visible="false" ForeColor="Red"
                            CssClass="font" Style="top: 26px; position: absolute;"></asp:Label>
                              <table style="width: 395px; height: 182px;">
                              <tr>
                                  <td>
                                     <center>
                                        <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                                            OnCellClick="FpSpread1_CellClick" Visible="false" OnPreRender="FpSpread1_SelectedIndexChanged"
                                            BorderWidth="1px" Width="592" Height="117" Style="top: 110px; position: absolute;
                                            left: 73px;">
                                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                                ButtonShadowColor="ControlDark">
                                            </CommandBar>
                                            <Sheets>
                                                <FarPoint:SheetView SheetName="Sheet1">
                                                </FarPoint:SheetView>
                                            </Sheets>
                                        </FarPoint:FpSpread>
                                    </center>
                                </td>
                              </tr>
                              </table>

                 </asp:Panel>
                 </center>
                <center>
                    <div id="popwindow1" runat="server" class="popupstyle popupheight1" style="display: none">
                        <asp:ImageButton ID="imagebtnpop1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                            Style="height: 30px; width: 30px; position: absolute; margin-top: 30px; margin-left: 450px;"
                            OnClick="btn_exit_Click" />
                        <br />
                        <br />
                        <div style="background-color: White; height: 550px; width: 920px; border: 5px solid #0CA6CA;
                            border-top: 30px solid #0CA6CA; border-radius: 10px;">
                            <br />
                            <center>
                                <div>
                                    <span class="fontstyleheader">More Attachments</span></div>
                            </center>
                            <br />
                            <div style="width: 850px; height: 425px;" class="table">
                                <br />
                                <div>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:FileUpload ID="FileUpload2" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:FileUpload ID="FileUpload3" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:FileUpload ID="FileUpload4" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:FileUpload ID="FileUpload5" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:FileUpload ID="FileUpload6" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </center>
                <%-------end of popup*-----%>
                <div id="div_staff" runat="server" style="display: none;">
                    <br />
                    <center><span Style="font-size: x-large; color: #008000;">Staff</span></center>
                    <center>
                        <table width="900px">
                            <tr>
                                <td align="right">
                                 <div class="maindivstyle" align="center" style="border-radius: 7px; width: 120px;
                                    height: 30px;">
                                    <asp:RadioButton ID="rb_stf_in" runat="server" Text="In" GroupName="s1" Checked="true" onchange="return save_staff_clear1();"/>

                                   
                                    <asp:RadioButton ID="rb_stf_out" runat="server" Text="Out" GroupName="s1" onchange="return save_staff_clear();" />
                                    </div>
                                </td>
                               
                                <td style="width:100"></td>
                                <td style="width:450px"></td>

                                <td>
                                 <div class="maindivstyle" align="center" style="border-radius: 7px; width: 360px;
                                    height: 40px;">
                                <table>
                                <tr>
                                <td>
                                    <asp:Label ID="lbl_stf_date" Text="Date" runat="server"></asp:Label>
                                </td>
                                <td>
                                 <asp:UpdatePanel ID="Updp_todate" runat="server">
                                        <ContentTemplate>
                                    <asp:TextBox ID="txt_stf_date" runat="server" CssClass="textbox txtheight textbox1" AutoPostBack="true" OnTextChanged="txt_stf_date_changed"></asp:TextBox>
                                      <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_stf_date" runat="server"
                                                CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            </ContentTemplate>
                                            </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_stf_time" Text="Time" runat="server"></asp:Label>
                                </td>
                                <td>
                                    
                                    <asp:DropDownList ID="ddl_stfhr" Width="50px" Height="25px" runat="server" CssClass="textbox textbox1">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddl_stfm" Width="50px" Height="25px" runat="server" CssClass="textbox textbox1">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddl_stfam" Width="50px" Height="25px" runat="server"
                                        CssClass="textbox textbox1">
                                        <asp:ListItem>AM</asp:ListItem>
                                        <asp:ListItem>PM</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                              
                                </tr>
                                </table>
                                </div>
                                </td>
                            </tr>
                        </table>
                    </center>
                    <br />
                    <div style="width: 900px;">
                        <div id="staffchngdiv" style="float: left;">
                            <table>
                                <tr>
                                    <td colspan="2" align="center">
                                     <asp:Label ID="lbl_chng_to_staff" ForeColor="#008000" Font-Bold="true" Text="To" runat="server" onfocus="return myFunction(this)" style="display:none;" ></asp:Label>
                                        <asp:Label ID="lbl_stud_from" ForeColor="#008000" Font-Bold="true" Text="From" runat="server" onfocus="return myFunction(this)" ></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:RadioButton ID="rb_current" runat="server" Text="Current Staff" GroupName="r1" Checked="true" onchange="return rbcurrentstaff();" />
                                        <asp:RadioButton ID="rb_old" runat="server" Text="Old Staff" GroupName="r1" onchange="return rbcurrentstaff1();" />
                                    </td>
                                </tr>                                 
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_name1" Text="Staff Name" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_name1" runat="server" onchange="return checkstaffname(this.value)" onkeyup="return checkstaffname(this.value)" onfocus="return myFunction(this)" CssClass="textbox txtheight5 textbox1"
                                            onblur="getstaff(this.value)" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txt_name1"
                                            FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ().-">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:AutoCompleteExtender ID="auto_staff" runat="server" DelimiterCharacters="" Enabled="True"
                                            ServiceMethod="Getstaffname" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                                            CompletionSetCount="10" ServicePath="" TargetControlID="txt_name1" CompletionListCssClass="autocomplete_completionListElement"
                                            CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                         <span style="color: Red;">*</span>
                                          <span id="rnomsgstaff"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_stfcode" Text="Staff Code" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_stf_code" runat="server" CssClass="textbox txtheight1" 
                                           onchange="return checkstaffcode(this.value)" onkeyup="return checkstaffcode(this.value)"   onblur="getstaffcode(this.value)" onfocus="return myFunction(this)" onkeydown = "return (event.keyCode!=13);"></asp:TextBox><%-- barath 19.04.17Enabled="false" BackColor="#ffffcc"--%>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender74" runat="server" TargetControlID="txt_stf_code"
                                            FilterType="numbers,custom" ValidChars=" .-">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters="" Enabled="True"
                                            ServiceMethod="Getstaffcode" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                                            CompletionSetCount="10" ServicePath="" TargetControlID="txt_stf_code" CompletionListCssClass="autocomplete_completionListElement"
                                            CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                         <span style="color: Red;">*</span>
                                          <span id="msgstaffcode"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_dept" Text="Department" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_dept" runat="server" CssClass="textbox txtheight5" Enabled="false" BackColor="#ffffcc"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_desigtn" Text="Designation" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_desigtn" runat="server" CssClass="textbox txtheigth3" Enabled="false" BackColor="#ffffcc"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_studtype1" Text="Staff Type" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_stafftype" runat="server" CssClass="textbox txtheight" Enabled="false" BackColor="#ffffcc"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_stf_add" Text="Address" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_stf_add" runat="server" CssClass="textbox txtheight5" Enabled="false" BackColor="#ffffcc">
                                            </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_stf_str" Text="Street" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_stf_str" runat="server" CssClass="textbox" Enabled="false" BackColor="#ffffcc"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_stf_city" Text="City" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_stf_city" runat="server" CssClass="textbox" Enabled="false" BackColor="#ffffcc"></asp:TextBox>
                                    </td>
                                </tr>
                                 <tr>
                                    <td>
                                        <asp:Label ID="lbl_stf_pin" Text="Pin Code" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_stf_pin" runat="server" CssClass="textbox" Enabled="false" BackColor="#ffffcc"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_stf_state" Text="State" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_stf_state" runat="server" CssClass="textbox" Enabled="false" BackColor="#ffffcc"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_stf_country" Text="Country" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_stf_country" runat="server" CssClass="textbox" Enabled="false" BackColor="#ffffcc"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_stf_ph" Text="Phone No" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_stf_ph" runat="server" CssClass="textbox" Enabled="false" BackColor="#ffffcc"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_stf_mail" Text="Mail ID" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_stf_mail" runat="server" CssClass="textboxchng txtheight5" Enabled="false" BackColor="#ffffcc"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div style="float: left; margin-left: 75px;">
                            <table>
                                <tr>
                                    <td colspan="2" align="center">
                                     <asp:Label ID="lbl_chng_from_staff" ForeColor="#008000" Font-Bold="true" Text="From" runat="server" style="display:none;" 
onfocus="return myFunction(this)" ></asp:Label>
                                        <asp:Label ForeColor="#008000" Font-Bold="true" ID="lbl_stf_to" Text="To" runat="server" 
onfocus="return myFunction(this)"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:CheckBox ID="cb_stf_dept" runat="server" Text="Department" onfocus="return myFunction(this)"
                                            onchange="return staff_checkchange1(this.value);" />
                                        <asp:CheckBox ID="cb_stf_indi" runat="server" Text="Individual" onchange="return staff_checkchange2(this.value);"
                                            onfocus="return myFunction();" />
                                    </td>
                                </tr>
                            </table>
                            <div id="div_stf_dept" runat="server" style="display: none" onfocus="return myFunction(this)">
                              <span id="deptmsg1"></span>
                               <span id="indimsg1"></span>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_stf_depto" Text="Department" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_stf_depto" runat="server" Width="190px" Height="20px" onchange="return checkdepartment1(this.value)" onkeyup="return checkdepartment1(this.value)"  onfocus="return myFunction(this)" CssClass="textbox1 textbox" onkeydown = "return (event.keyCode!=13);" ></asp:TextBox>
                                          
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txt_stf_depto"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="auto_staffdept1" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getdept" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_stf_depto"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <asp:Button ID="btn_add" runat="server" CssClass="textbox textbox1 btn1" Text="Add"
                                                OnClientClick="return staff_change();" />
                                                <asp:Button ID="btn_staf_deptto_rmv" Width="58px" runat="server" CssClass="textbox textbox1 btn1" Text="Remove"
                                                OnClientClick="return staff_change0();" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_stf_depto1" runat="server" Style="display: none;" onchange="return checkdepartment1(this.value)" onkeyup="return checkdepartment1(this.value)"  onfocus="return myFunction(this)"
                                                Width="190px" Height="20px" CssClass="textbox1 textbox"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txt_stf_depto1"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="auto_staffdept2" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getdept" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_stf_depto1"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_stf_cc" Text="Cc" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_stf_cc" runat="server" Width="190px" onchange="return checkdepartment1(this.value)" onkeyup="return checkdepartment1(this.value)"  onfocus="return myFunction(this)" Height="20px" CssClass="textbox1 textbox" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txt_stf_cc"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="auto_staffdept3" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getdept" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_stf_cc"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <asp:Button ID="btn_add1" runat="server" OnClientClick="return staff_change1();"
                                                CssClass="textbox textbox1 btn1" Text="Add" />
                                                <asp:Button ID="btn_staf_deptcc_rmv" Width="58px" runat="server" CssClass="textbox textbox1 btn1" Text="Remove"
                                                OnClientClick="return staff_change11();" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_stf_cc1" runat="server" onchange="return checkdepartment1(this.value)" onkeyup="return checkdepartment1(this.value)" Style="display: none;" onfocus="return myFunction(this)"
                                                Width="190px" Height="20px" CssClass="textbox1 textbox"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txt_stf_cc1"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="auto_staffdept4" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getdept" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_stf_cc1"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="div_stf_indi" runat="server" style="display: none; margin-left:11px" onfocus="return myFunction(this)">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_stf_indito" Text="Individual" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_stf_indito" runat="server" Width="190px"  onfocus="return myFunction(this)" onchange="return checkindiv1(this.value)" onkeyup="return checkindiv1(this.value)" Height="20px" CssClass="textbox1 textbox"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender51" runat="server" TargetControlID="txt_stf_indito"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom,Numbers" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="autostaffindi1" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getstaff" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_stf_indito"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <asp:Button ID="btn_add2" runat="server" OnClientClick="return staff_change2();"
                                                CssClass="textbox textbox1 btn1" Text="Add" />
                                                 <asp:Button ID="btn_staff_indito_rmv" Width="58px" runat="server" CssClass="textbox textbox1 btn1" Text="Remove"
                                                OnClientClick="return staff_change22();" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_stf_indito1" runat="server" onfocus="return myFunction(this)"
                                                Width="190px" Height="20px" onchange="return checkindiv1(this.value)" onkeyup="return checkindiv1(this.value)" CssClass="textbox1 textbox" Style="display: none;">
                                            </asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender52" runat="server" TargetControlID="txt_stf_indito1"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom,Numbers" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="autostaffindi2" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getstaff" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_stf_indito1"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_stf_indicc" Text="Cc" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_stf_indicc" runat="server" Width="190px" onchange="return checkindiv1(this.value)" onkeyup="return checkindiv1(this.value)"  onfocus="return myFunction(this)" Height="20px" CssClass="textbox1 textbox"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender53" runat="server" TargetControlID="txt_stf_indicc"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom,Numbers" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="autostaffindi3" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getstaff" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_stf_indicc"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <asp:Button ID="btn_add3" runat="server" OnClientClick="return staff_change3();"
                                                CssClass="textbox textbox1 btn1" Text="Add" />
                                                 <asp:Button ID="btn_staf_indicc_rmv" Width="58px" runat="server" CssClass="textbox textbox1 btn1" Text="Remove"
                                                OnClientClick="return staff_change33();" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_stf_indicc1" runat="server" onchange="return checkindiv1(this.value)" onkeyup="return checkindiv1(this.value)" onfocus="return myFunction(this)"
                                                Style="display: none;" Width="190px" Height="20px" CssClass="textbox1 textbox"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender54" runat="server" TargetControlID="txt_stf_indicc1"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom,Numbers" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="autostaffindi4" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getstaff" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_stf_indicc1"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                    <div style="float: left; margin-left: 140px;">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_stf_subj" Text="Subject" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_stf_subj" runat="server" CssClass="textboxchng textbox1 txtheight6" MaxLength="200"  onfocus="return myFunction(this)"  Width="698px" onkeydown = "return (event.keyCode!=13);">
                                        </asp:TextBox>
                                           <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender108" runat="server" TargetControlID="txt_stf_subj"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom,Numbers" ValidChars=" .-&@/*">
                                            </asp:FilteredTextBoxExtender>
                                         <span style="color: Red;">*</span>
                                </td>
                            </tr>
                            <%--onkeyup="myFunCaps(this.id)"--%>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_stf_docpri" Text="Document Priority" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_stf_docpri" runat="server" CssClass="textbox  ddlheight4"
                                        Style="float: left;" onchange="changestaff1(this)" onfocus="return myFunction(this)">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txt_doc" runat="server" CssClass="textbox txtheight4 textbox1" Style="display: none;
                                        float: left;" onfocus="return myFunction(this)"></asp:TextBox> <%--delsi--%>
                                         <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender65" runat="server" TargetControlID="txt_doc"
                                                FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" -">
                                            </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_stf_delivry" Text="Received Mode" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_stf_delivry" runat="server" CssClass="textbox  ddlheight4"
                                        Style="float: left;" onchange="change2staff(this)" onfocus="return myFunction(this)">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txt_deliv" runat="server" CssClass="textbox txtheight4 textbox1"
                                        Style="display: none; float: left;" onfocus="return myFunction(this)"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_stf_refno" Text="Ref No" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_stf_refno" runat="server" CssClass="textbox txtheight1 textbox1" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                                     <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender76" runat="server" TargetControlID="txt_stf_refno"
                                                 FilterType="Custom,UppercaseLetters,LowercaseLetters,numbers" ValidChars=" .#$*&/-@">
                                            </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_stf_atch" Text="Attachments" runat="server"></asp:Label>
                                </td>
                                <%--<td>--%>
                                  <%--  <asp:FileUpload ID="FileUpload1" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" />
                                    <asp:Button ID="btn_stf_more" runat="server" CssClass="textbox textbox1 btn2" Text="More" OnClientClick="return browse_change();"
                                        OnClick="btn_stf_more_Click" Visible="false" />--%>


                                        <td align="left">
                                
                    
                                <div id="div_certificate_Grid1" runat="server" >
                                <center>
                                  <span style="color: White; font-weight: bold;"></span>
                                  </center>
                                            <asp:GridView ID="staffupload_Grid" runat="server" AutoGenerateColumns="false" Visible="true">
                                                <Columns>
                                                                                            
                                                    <asp:TemplateField HeaderText="No of Files" HeaderStyle-ForeColor="White"  HeaderStyle-BackColor="Brown"  HeaderStyle-HorizontalAlign="center">
                                                        <ItemTemplate>
                                                          
                                                            <asp:Label ID="lbl_certificate1" ReadOnly="true" runat="server" Text='<%#Eval("Files") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Browse"  HeaderStyle-ForeColor="White" HeaderStyle-BackColor="Brown" HeaderStyle-HorizontalAlign="center">
                                                        <ItemTemplate>
                                                        <asp:Label ID="lbl_mandatory1" runat="server" Text="*"></asp:Label>
                                                            <asp:FileUpload ID="file_uplod1" runat="server" />
                                                            
                                                        </ItemTemplate>
                                                       
                                                    </asp:TemplateField>

                                                </Columns>
                                            </asp:GridView>
                                        </div> 

                                         
                                      <asp:Button ID="staff_addrow" runat="server" CssClass="textbox textbox1 type" BackColor="Brown"
                                    ForeColor="White"  Visible="true" Width="81px" Height="26px" Text="Add Rows"
                                     OnClick="btnaddrows1_Click" />
                                 
                                         </td>
                                     
                            </tr>
                             <tr>
                            <td>
                           
                         <asp:Label ID="lbl_chargesstaff" runat="server" Text="Charges"></asp:Label></td>
                         <td>
                         <asp:TextBox ID="txt_chargesstaff" runat="server" CssClass="textbox textbox1 txtheight1" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                          <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender115" runat="server" TargetControlID="txt_chargesstaff"
                                            FilterType="numbers" ValidChars="">
                                        </asp:FilteredTextBoxExtender></td>
                            </tr>
                             <tr>
                                        <td>
                                        <asp:Label ID="lbl_paymentstaff" Text="Payment" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                        <asp:CheckBox ID="cb_paymentstaff" runat="server" onchange="return staffpayment(this.value)" onfocus="return myFunction(this)"  /></td>
                            </tr>
                        </table>

                        <div id="imgdiv4" runat="server" visible="false" style="height: 100%; z-index: 1000;
                                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                left: 0px;">
                                <center>
                                    <div id="panel_erroralert1" runat="server" class="table" style="background-color: White;
                                        height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                        margin-top: 200px; border-radius: 10px;">
                                        <center>
                                            <table style="height: 100px; width: 100%">
                                                <tr>
                                                    <td align="center">
                                                        <asp:Label ID="lbl_erroralert1" runat="server" Style="color: Red;" Font-Bold="true"
                                                            Font-Size="Medium"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <center>
                                                            <asp:Button ID="btn_erroralert1" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                                width: 65px;" OnClick="btnerrclose1_Click" Text="Ok" runat="server" />
                                                        </center>
                                                    </td>
                                                </tr>
                                            </table>
                                        </center>
                                    </div>
                                </center>
                            </div>
                             <div id="div_staff_payment" runat="server" style="display:none" class="maindivstyle">
  
                             <table><tr> <%--delsi--%>
                            <td>
                          
                          
                          
                            <asp:RadioButton ID="rdb_staff_cheque" runat="server" Checked="true" GroupName="pay1" Text="Cheque" onchange="return chequechangestaff(this.value)" onfocus="return myFunction(this)"  />

                           
                            <asp:RadioButton ID="rdb_staff_dd" runat="server" GroupName="pay1" Text="DD" onchange="return ddchangestaff(this.value)" onfocus="return myFunction(this)"  />
                            </td>
                            <td>
                            

                            <asp:Label ID="lbl_staff_bank" Text="Bank" runat="server"></asp:Label>

                            </td>
                            <td>
                          
                            <asp:DropDownList ID="ddl_staff_bank" runat="server" CssClass="textbox  ddlheight4"
                                         onchange="changebank1(this)" onfocus="return myFunction(this)"></asp:DropDownList>
                                         </td>
                                         <td>

                            <asp:TextBox ID="txt_bank" runat="server" CssClass="textbox txtheight4 textbox1" Style="display: none;
                                        " onfocus="return myFunction(this)"></asp:TextBox>
                                        </td>
                                        <td>
                          
                            <asp:Label ID="lbl_staff_bankbranch" Text="Branch" runat="server"></asp:Label>
                            </td>
                            <td>
                            <asp:TextBox ID="txt_staff_bankbarnch" runat="server" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                             </td></tr></table>
                           </div>
                         
                        <br />
                            <div id="div_staff_cheque" runat="server" style="display:none" class="maindivstyle">
                            <table>
                            <tr>
                            <td><asp:Label ID="lbl_staff_chequename" runat="server" Text="Name"></asp:Label></td>
                            <td><asp:TextBox ID="txt_staff_chequename" runat="server" CssClass="txtheight4 textbox textbox1"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender126" runat="server" TargetControlID="txt_staff_chequename"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender></td>
                             <td><asp:Label ID="lbl_staff_chequeno" runat="server" Text="Cheque No"></asp:Label></td>
                            <td><asp:TextBox ID="txt_staff_cheque" runat="server" CssClass="txtheight textbox textbox1"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender127" runat="server" TargetControlID="txt_staff_cheque"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom,numbers" ValidChars=" ">
                                </asp:FilteredTextBoxExtender></td>
                             <td><asp:Label ID="lbl_staff_chequedate" runat="server" Text="Date"></asp:Label></td>
                            <td><asp:TextBox ID="txt_staff_chequedate" runat="server" CssClass="txtheight textbox textbox1"></asp:TextBox>
                             <asp:CalendarExtender ID="CalendarExtender8" TargetControlID="txt_staff_chequedate" runat="server"
                                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender></td>
                             <td><asp:Label ID="lbl_staff_chequeamt" runat="server" Text="Amount"></asp:Label></td>
                            <td><asp:TextBox ID="txt_staff_chequeamt" runat="server" CssClass="txtheight2 textbox textbox1"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender128" runat="server" TargetControlID="txt_staff_chequeamt"
                                    FilterType="Numbers" ValidChars="">
                                </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            </table>
                            </div>
                              <div id="div_staff_dd" runat="server" style="display:none" class="maindivstyle">
                            <table>
                            <tr>
                            <td><asp:Label ID="lbl_staffddname" runat="server" Text="Name"></asp:Label></td>
                            <td><asp:TextBox ID="txt_staffddname" runat="server" CssClass="txtheight4 textbox textbox1"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender129" runat="server" TargetControlID="txt_staffddname"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender></td>
                             <td><asp:Label ID="lbl_staff_ddno" runat="server" Text="DD No"></asp:Label></td>
                            <td><asp:TextBox ID="txt_staff_ddno" runat="server" CssClass="txtheight textbox textbox1"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender130" runat="server" TargetControlID="txt_staff_ddno"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom,numbers" ValidChars=" ">
                                </asp:FilteredTextBoxExtender></td>
                             <td><asp:Label ID="lbl_staff_dddate" runat="server" Text="Date"></asp:Label></td>
                            <td><asp:TextBox ID="txt_staff_dddate" runat="server" CssClass="txtheight textbox textbox1"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender9" TargetControlID="txt_staff_dddate" runat="server"
                                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender></td>
                             <td><asp:Label ID="lbl_staff_ddamt" runat="server" Text="Amount"></asp:Label></td>
                            <td><asp:TextBox ID="txt_staff_ddamt" runat="server" CssClass="txtheight2 textbox textbox1"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender131" runat="server" TargetControlID="txt_staff_ddamt"
                                    FilterType="Numbers" ValidChars="">
                                </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            </table>
                            </div>

                    </div>
                    <div id="div2" runat="server" style="float: left; margin-top:33px; margin-left: 400px" onfocus="return myFunction(this)">
                        <asp:Button ID="btnstaff_save" runat="server" CssClass="textbox textbox1 btn2" Text="Save" OnClientClick="return validstaff()"
                            OnClick="btnstaff_save_Click" />
                        <asp:Button ID="btnstaff_clear" runat="server" CssClass="textbox textbox1 btn2" Text="Clear" OnClientClick="return staff_div_clear();"
                            OnClick="btnstaff_clear_Click" /></div>
                </div>
                <%-----end of staff-----%>
                <div id="div_parent" runat="server" style="display: none;">
                    <br />
                    <center><span Style="font-size: x-large; color: #008000;">Parents</span></center>
                    <center>
                        <table width="900px">
                            <tr>
                                <td align="right">
                                 <div class="maindivstyle" align="center" style="border-radius: 7px; width: 120px;
                                    height: 30px;">
                                    <asp:RadioButton ID="rb_par_in" runat="server" Text="In" GroupName="c" Checked="true" onchange=" return save_parent_clear1();" />
                                    
                                    <asp:RadioButton ID="rb_par_out" runat="server" Text="Out" GroupName="c" onchange=" return save_parent_clear();" />
                                    </div>
                                </td>
                                <td style="width:100"></td>
                                <td style="width:450px"></td>
                                <td>
                                 <div class="maindivstyle" align="center" style="border-radius: 7px; width: 360px;
                                    height: 40px;">
                                   
                                    <table>
                                     <tr>
                                <td>
                                    <asp:Label ID="lbl_pardate" Text="Date" runat="server"></asp:Label>
                                </td>
                                <td>
                                 <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                    <asp:TextBox ID="txt_pardate" runat="server" CssClass="textbox txtheight textbox1" AutoPostBack="true" OnTextChanged="txt_pardate_changed"></asp:TextBox>
                                      <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_pardate" runat="server"
                                                CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            </ContentTemplate>
                                            </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_partime" Text="Time" runat="server"></asp:Label>
                                </td>
                              
                                     <td>
                                    <asp:DropDownList ID="ddl_parhr" Width="50px" Height="25px" runat="server" CssClass="textbox textbox1">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddl_parm" Width="50px" Height="25px" runat="server" CssClass="textbox textbox1">
                                    </asp:DropDownList>
                                   
                                    
                                    <asp:DropDownList ID="ddl_param" Width="50px" Height="25px" runat="server"
                                        CssClass="textbox textbox1">
                                        <asp:ListItem>AM</asp:ListItem>
                                        <asp:ListItem>PM</asp:ListItem>
                                    </asp:DropDownList>
                               
                                </td>
                                </tr>
                                </table>
                                </div>
                                </td>
                            </tr>
                        </table>
                    </center>
                    <br />
                    <div style="width: 900px;">
                        <div id="parchngdiv" style="float: left;">
                            <table>
                                <tr>
                                    <td colspan="2" align="center">
                                     <asp:Label ID="lbl_chng_to_par" ForeColor="#008000" Font-Bold="true" Text="To" runat="server" style="display:none;" onfocus="return myFunction(this)"></asp:Label>
                                        <asp:Label ID="lbl_parfrom" ForeColor="#008000" Font-Bold="true" Text="From" runat="server" onfocus="return myFunction(this)"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <%-- <asp:RadioButton ID="RadioButton3" runat="server" Text="Current" GroupName="c1" Checked="true"/>
                                        <asp:RadioButton ID="RadioButton4" runat="server" Text="Old" GroupName="c1" />--%>
                                        <asp:RadioButton ID="rdo_parent_current" runat="server" Text="Current Student's Parent" GroupName="same1" onchange=" return rbcurrentparent();"/>
                                        <asp:RadioButton ID="rdo_parent_old" runat="server" Text="Old Student's Parent" GroupName="same1" onchange=" return rbcurrentparent1();" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    <asp:Label ID="lbl_par_reg" Text="Reg No" runat="server" style="display: none" onfocus="return myFunction(this)"></asp:Label>
                                        <asp:Label ID="lbl_rollno" Text="Roll No" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_rollno" runat="server" CssClass="textbox txtheight1 textbox1" onchange="return checkrno1(this.value)"  onkeyup="return checkrno1(this.value)"
                                            onblur="getparent1(this.value)"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender30" runat="server" TargetControlID="txt_rollno"
                                            FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:AutoCompleteExtender ID="auto_parent_roll" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_rollno"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                         <span id="errmsg"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_studname" Text="Student Name" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_studname" runat="server" CssClass="textbox txtheight5  textbox1"
                                            onblur="getparent(this.value)"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender31" runat="server" TargetControlID="txt_studname"
                                            FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_studname"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_fname" Text="Father's Name" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_fname" runat="server"  onfocus="return myFunction(this)" CssClass="textbox txtheight5 textbox1"
                                            onblur="getparent2(this.value)"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender64" runat="server" TargetControlID="txt_fname"
                                            FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getparentname" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_fname"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                         <span style="color: Red;">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_mob" Text="Mobile No" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_mob" runat="server" CssClass="textbox  textbox1" onblur="getparent3(this.value)" MaxLength="12"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender63" runat="server" TargetControlID="txt_mob"
                                            FilterType="numbers,custom" ValidChars=" -,">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:AutoCompleteExtender ID="auto_parent_mob" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getparentmob" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_mob"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_addr" Text="Address" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_addr" runat="server" Enabled="false" BackColor="#ffffcc" CssClass="textbox txtheight5"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_street2" Text="Street" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_street2" runat="server" CssClass="textbox" Enabled="false" BackColor="#ffffcc"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_par_city" Text="City" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_par_city" runat="server" CssClass="textbox" Enabled="false" BackColor="#ffffcc"></asp:TextBox>
                                    </td>
                                </tr>
                                 <tr>
                                    <td>
                                        <asp:Label ID="lbl_par_pin" Text="Pin Code" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_par_pin" runat="server" CssClass="textbox" Enabled="false" BackColor="#ffffcc"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_par_state" Text="State" runat="server" ></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_par_state" runat="server" CssClass="textbox" Enabled="false" BackColor="#ffffcc"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_par_country" Text="Country" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_par_country" runat="server" CssClass="textbox" Enabled="false" BackColor="#ffffcc"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_par_phno" Text="Phone No" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_par_phno" runat="server" CssClass="textbox" Enabled="false" BackColor="#ffffcc"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_par_mail" Text="Mail ID" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_par_mail" runat="server" CssClass="textboxchng txtheight5" Enabled="false" BackColor="#ffffcc"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div style="float: left; margin-left: 50px;">
                            <table>
                                <tr>
                                    <td colspan="2" align="center">
                                    <asp:Label  ForeColor="#008000" Font-Bold="true" ID="lbl_chng_from_par" Text="From" runat="server" style="display:none;" onfocus="return myFunction(this)"></asp:Label>
                                        <asp:Label ForeColor="#008000" Font-Bold="true" ID="lbl_parto" Text="To" runat="server" onfocus="return myFunction(this)"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:CheckBox ID="cb_par_dept" runat="server" Text="Department" onfocus="return myFunction(this)"
                                            onchange="return parnt_checkchange1(this.value);" />
                                        <asp:CheckBox ID="cb_par_indi" runat="server" Text="Staff" onfocus="return myFunction(this)"
                                            onchange="return parnt_checkchange2(this.value);" />
                                        <asp:CheckBox ID="cb_par_stud" runat="server" Text="Student" onfocus="return myFunction(this)"
                                            onchange="return parnt_checkchange3(this.value);" />
                                    </td>
                                </tr>
                            </table>
                            <div id="div_parnt_dept" runat="server" style="display: none" onfocus="return myFunction(this)">
                              <span id="deptmsg2"></span>
                               <span id="indimsg2"></span>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lal_par_depto" Text="Department" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_par_depto" runat="server" onfocus="return myFunction(this)" Width="250px" Height="20px" onchange="return checkdepartment2(this.value)" onkeyup="return checkdepartment2(this.value)" CssClass="textbox1 textbox"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" TargetControlID="txt_par_depto"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="autoparent1" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getdept" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_par_depto"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <asp:Button ID="btn_paradd" runat="server" CssClass="textbox textbox1 btn1" Text="Add"
                                                OnClientClick="return parnt_change();" />
                                                <asp:Button ID="btn_stafff_deptto_rmv" Width="58px" runat="server" CssClass="textbox textbox1 btn1" Text="Remove"
                                                OnClientClick="return parnt_change0();" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_par_depto1" runat="server" Width="250px" onchange="return checkdepartment2(this.value)" onkeyup="return checkdepartment2(this.value)" Height="20px" CssClass="textbox1 textbox"
                                                Style="display: none;" onfocus="return myFunction(this)"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" TargetControlID="txt_par_depto1"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="autoparent2" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getdept" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_par_depto1"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_par_cc" Text="Cc" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_par_cc" runat="server" onfocus="return myFunction(this)" onchange="return checkdepartment2(this.value)" onkeyup="return checkdepartment2(this.value)" Width="250px" Height="20px" CssClass="textbox1 textbox"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" TargetControlID="txt_par_cc"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="autoparent3" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getdept" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_par_cc"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <asp:Button ID="btn_paradd1" runat="server" CssClass="textbox textbox1 btn1" Text="Add"
                                                OnClientClick="return parnt_change1();" />
                                                <asp:Button ID="btn_staf_depcc_rmv" Width="58px" runat="server" CssClass="textbox textbox1 btn1" Text="Remove"
                                                OnClientClick="return parnt_change11();" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_par_cc1" runat="server" Width="250px" Height="20px" onchange="return checkdepartment2(this.value)" onkeyup="return checkdepartment2(this.value)" CssClass="textbox1 textbox"
                                                Style="display: none;" onfocus="return myFunction(this)"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" TargetControlID="txt_par_cc1"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="autoparent4" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getdept" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_par_cc1"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            
                            <div id="div_parnt_indi" runat="server" style="display: none; margin-left:5px" onfocus="return myFunction(this)">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_par_indi" Text="Staff" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_par_indi" runat="server" onfocus="return myFunction(this)" onchange="return checkindiv2(this.value)" onkeyup="return checkindiv2(this.value)" Width="290px" Height="20px" CssClass="textbox1 textbox"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender45" runat="server" TargetControlID="txt_par_indi"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom,Numbers" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="autoparindi1" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getstaff" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_par_indi"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <asp:Button ID="btn_par_add2" runat="server" CssClass="textbox textbox1 btn1" Text="Add"
                                                OnClientClick="return parnt_change2();" />
                                                 <asp:Button ID="btn_staf_indito_rmv" Width="58px" runat="server" CssClass="textbox textbox1 btn1" Text="Remove"
                                                OnClientClick="return parnt_change22();" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_par_indi1" Style="display: none;" onfocus="return myFunction(this)"
                                                runat="server" Width="290px" Height="20px" onchange="return checkindiv2(this.value)" onkeyup="return checkindiv2(this.value)" CssClass="textbox1 textbox"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender46" runat="server" TargetControlID="txt_par_indi1"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom,Numbers" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="autoparindi2" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getstaff" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_par_indi1"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_par_cc2" Text="Cc" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_par_cc2" runat="server" onfocus="return myFunction(this)" onchange="return checkindiv2(this.value)" onkeyup="return checkindiv2(this.value)" Width="290px" Height="20px" CssClass="textbox1 textbox"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender47" runat="server" TargetControlID="txt_par_cc2"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom,Numbers" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="autoparindi3" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getstaff" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_par_cc2"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <asp:Button ID="btn_par_add3" runat="server" CssClass="textbox textbox1 btn1" Text="Add"
                                                OnClientClick="return parnt_change3();" />
                                                 <asp:Button ID="btn_staf_indicccc_rmv" Width="58px" runat="server" CssClass="textbox textbox1 btn1" Text="Remove"
                                                OnClientClick="return parnt_change33();" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_par_cc3" Style="display: none;" onfocus="return myFunction(this)"
                                                runat="server" Width="290px" onchange="return checkindiv2(this.value)" onkeyup="return checkindiv2(this.value)" Height="20px" CssClass="textbox1 textbox"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender48" runat="server" TargetControlID="txt_par_cc3"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom,Numbers" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="autoparindi4" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getstaff" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_par_cc3"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="div_par_stud" runat="server" style="display: none; margin-left:8px" onfocus="return myFunction(this)">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_par_stud1" Text="Student" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_par_studto" runat="server" Width="270px" Height="20px" CssClass="textbox1 textbox"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender49" runat="server" TargetControlID="txt_par_studto"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom,Numbers" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="autoparstud1" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="GetName" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_par_studto"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <asp:Button ID="Button5" runat="server" CssClass="textbox textbox1 btn1" Text="Add"
                                                OnClientClick="return parent_change4();" />
                                                  <asp:Button ID="btn_staff_studto_rmv" Width="58px" runat="server" CssClass="textbox textbox1 btn1" Text="Remove"
                                                OnClientClick="return parent_change44();" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_par_studto1" runat="server" Width="270px" Height="20px" CssClass="textbox1 textbox"
                                                Style="display: none;" onfocus="return myFunction(this)"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender50" runat="server" TargetControlID="txt_par_studto1"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom,Numbers" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="GetName" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_par_studto1"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                  
                    <div style="float: left; margin-left: 140px;">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_par_sub" Text="Subject" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_par_sub" runat="server" onfocus="return myFunction(this)" MaxLength="20" Width="700px" onkeyup="myFunCaps(this.id)" CssClass="textboxchng textbox1 txtheight6"></asp:TextBox>
                                 <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender109" runat="server" TargetControlID="txt_par_sub"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom,Numbers" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                         <span style="color: Red;">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_par_doc" Text="Document Priority" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_par_doc" runat="server" CssClass="textbox  ddlheight4"
                                        Style="float: left;" onchange="change1parent(this)" onfocus="return myFunction(this)">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txt_par_doc1" runat="server" CssClass="textbox txtheight4 textbox1"
                                        Style="display: none; float: left;" onfocus="return myFunction(this)"></asp:TextBox>
                                         <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender66" runat="server" TargetControlID="txt_par_doc1"
                                                FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_par_delivry" Text="Delivery Mode" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_par_delivry" runat="server" CssClass="textbox  ddlheight4"
                                        Style="float: left;" onchange="change2parent(this)" onfocus="return myFunction(this)">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txt_par_deliv1" runat="server" CssClass="textbox txtheight4 textbox1"
                                        Style="display: none; float: left;" onfocus="return myFunction(this)"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_par_refno" Text="Ref No" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_par_refno" runat="server" CssClass="textbox txtheight1 textbox1"></asp:TextBox>
                                     <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender77" runat="server" TargetControlID="txt_par_refno"
                                                 FilterType="Custom,UppercaseLetters,LowercaseLetters,numbers" ValidChars=" .#$*&">
                                            </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_par_atch" Text="Attachments" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:FileUpload ID="FileUpload_parnt" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" />
                                    <asp:Button ID="btn_par_more" Visible="false" runat="server" CssClass="textbox textbox1 btn2" Text="More" OnClientClick="return browse_change();"
                                        OnClick="btn_par_more_Click" />
                                </td>
                            </tr>
                             <tr>
                            <td>
                         
                         <asp:Label ID="lbl_chargespar" runat="server" Text="Charges"></asp:Label></td>
                         <td>
                         <asp:TextBox ID="txt_chargespar" runat="server" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                          <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender116" runat="server" TargetControlID="txt_chargespar"
                                            FilterType="numbers" ValidChars="">
                                        </asp:FilteredTextBoxExtender></td>
                            </tr>
                             <tr>
                                        <td>
                                        <asp:Label ID="lbl_paymentpar" Text="Payment" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                        <asp:CheckBox ID="cb_paymentpar" runat="server"  onchange="return parentpayment(this.value)" onfocus="return myFunction(this)" /></td>
                            </tr>
                        </table>

                             <table ><tr>
                            <td>
                           <div id="div_par_payment" runat="server" style="display:none" class="maindivstyle">
                          
                            <asp:RadioButton ID="rdo_par_cheque" runat="server" Checked="true" GroupName="pay2" Text="Cheque" onchange="return chequechangepar(this.value)" onfocus="return myFunction(this)"  />
                           
                            <asp:RadioButton ID="rdo_pardd" runat="server" GroupName="pay2" Text="DD" onchange="return ddchangepar(this.value)" onfocus="return myFunction(this)"  />
                            <asp:Label ID="lbl_par_bank" Text="Bank" runat="server"></asp:Label>
                            <asp:DropDownList ID="ddl_par_bank" runat="server" CssClass="ddlheight1 textbox textbox1"></asp:DropDownList>
                            <asp:Label ID="lbl_par_bankbanch" Text="Branch" runat="server"></asp:Label>
                            <asp:TextBox ID="txt_par_bankbanch" runat="server" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                           </div>
                         
                        </td></tr></table>
                        <br />
                            <div id="div_par_cheque" runat="server" style="display:none" class="maindivstyle">
                            <table>
                            <tr>
                            <td><asp:Label ID="lbl_par_chq_name" runat="server" Text="Name"></asp:Label></td>
                            <td><asp:TextBox ID="txt_par_chq_name" runat="server" CssClass="txtheight4 textbox textbox1"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender132" runat="server" TargetControlID="txt_par_chq_name"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender></td>
                             <td><asp:Label ID="lbl_par_chq_no" runat="server" Text="Cheque No"></asp:Label></td>
                            <td><asp:TextBox ID="txt_par_chq_no" runat="server" CssClass="txtheight textbox textbox1"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender133" runat="server" TargetControlID="txt_par_chq_no"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom,numbers" ValidChars=" ">
                                </asp:FilteredTextBoxExtender></td>
                             <td><asp:Label ID="lbl_par_chequedate" runat="server" Text="Date"></asp:Label></td>
                            <td><asp:TextBox ID="txt_par_chequedate" runat="server" CssClass="txtheight textbox textbox1"></asp:TextBox>
                             <asp:CalendarExtender ID="CalendarExtender10" TargetControlID="txt_par_chequedate" runat="server"
                                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender></td>
                             <td><asp:Label ID="lbl_par_chq_amt" runat="server" Text="Amount"></asp:Label></td>
                            <td><asp:TextBox ID="txt_par_chq_amt" runat="server" CssClass="txtheight2 textbox textbox1"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender134" runat="server" TargetControlID="txt_par_chq_amt"
                                    FilterType="Numbers" ValidChars="">
                                </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            </table>
                            </div>
                              <div id="div_par_dd" runat="server" style="display:none" class="maindivstyle">
                            <table>
                            <tr>
                            <td><asp:Label ID="lbl_par_ddname" runat="server" Text="Name"></asp:Label></td>
                            <td><asp:TextBox ID="txt_par_ddname" runat="server" CssClass="txtheight4 textbox textbox1"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender135" runat="server" TargetControlID="txt_par_ddname"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender></td>
                             <td><asp:Label ID="lbl_parddno" runat="server" Text="DD No"></asp:Label></td>
                            <td><asp:TextBox ID="txt_parddno" runat="server" CssClass="txtheight textbox textbox1"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender136" runat="server" TargetControlID="txt_parddno"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom,numbers" ValidChars=" ">
                                </asp:FilteredTextBoxExtender></td>
                             <td><asp:Label ID="lbl_pardddate" runat="server" Text="Date"></asp:Label></td>
                            <td><asp:TextBox ID="txt_pardddate" runat="server" CssClass="txtheight textbox textbox1"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender11" TargetControlID="txt_pardddate" runat="server"
                                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender></td>
                             <td><asp:Label ID="lbl_par_ddamt" runat="server" Text="Amount"></asp:Label></td>
                            <td><asp:TextBox ID="txt_par_ddamt" runat="server" CssClass="txtheight2 textbox textbox1"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender137" runat="server" TargetControlID="txt_par_ddamt"
                                    FilterType="Numbers" ValidChars="">
                                </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            </table>
                            </div>
                    </div>
                    <div id="div3" runat="server" style="float: left; margin-top:33px; margin-left: 400px" onfocus="return myFunction(this)">
                        <asp:Button ID="btnparent_save" runat="server" CssClass="textbox textbox1 btn2" Text="Save" OnClientClick="return validpar()"
                            OnClick="btnparent_save_Click" />
                        <asp:Button ID="btnparentclear" runat="server" CssClass="textbox textbox1 btn2" Text="Clear" OnClientClick="return parent_div_clear();"
                            OnClick="btnparentclear_Click" /></div>
                         </div>   
              

                <%-------end of parent-----%>
                <div id="div_company" runat="server" style="display: none;">
                    <br />
                    <center><span Style="font-size: x-large; color: #008000;">Company</span></center>
                    <center>
                        <table width="900px">
                            <tr>
                                <td align="right">
                                <div class="maindivstyle" align="center" style="border-radius: 7px; width: 120px;
                                    height: 30px;">
                                    <asp:RadioButton ID="rb_comp_in" runat="server" Text="In" GroupName="c0" Checked="true" onchange="return save_company_clear1();" />
                                    
                                    <asp:RadioButton ID="rb_comp_out" runat="server" Text="Out" GroupName="c0" onchange="return save_company_clear();" />
                                    </div>
                                </td>
                               
                                <td style="width:100"></td>
                                <td style="width:450px"></td>

                                <td>
                                 <div class="maindivstyle" align="center" style="border-radius: 7px; width: 360px;
                                    height: 40px;">
                                    <table><tr>
                                <td>
                                    <asp:Label ID="lbl_com_date" Text="Date" runat="server"></asp:Label>
                                </td>
                                <td>
                                 <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                    <asp:TextBox ID="txt_com_date" runat="server" CssClass="textbox txtheight  textbox1" AutoPostBack="true" OnTextChanged="txt_com_date_changed"></asp:TextBox>
                                      <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txt_com_date" runat="server"
                                                CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            </ContentTemplate>
                                            </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_com_time" Text="Time" runat="server"></asp:Label>
                                </td>
                                <td>
                                 <asp:DropDownList ID="ddl_comhr" Width="50px" Height="25px" runat="server" CssClass="textbox textbox1">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddl_commi" Width="50px" Height="25px" runat="server" CssClass="textbox textbox1">
                                    </asp:DropDownList>
                                   
                                    
                                    <asp:DropDownList ID="ddl_comam" Width="50px" Height="25px" runat="server"
                                        CssClass="textbox textbox1">
                                        <asp:ListItem>AM</asp:ListItem>
                                        <asp:ListItem>PM</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                </tr></table>
                                </div>
                                </td>
                            </tr>
                        </table>
                    </center>
                    <br />
                    <div style="width: 900px;">
                        <div id="compchngdiv" style="float: left;">
                            <table>
                                <tr>
                                    <td colspan="2" align="center">
                                     <asp:Label ID="lbl_chng_to_com" ForeColor="#008000" Font-Bold="true" Text="To" runat="server" style="display:none;" onfocus="return myFunction(this)"></asp:Label>
                                        <asp:Label ID="lbl_com_from" ForeColor="#008000" Font-Bold="true" Text="From" runat="server" onfocus="return myFunction(this)"></asp:Label>
                                    </td>
                                </tr>

                                 <tr>
                                    <td>
                                        <asp:Label ID="lbl_pername" Text="Person Name" runat="server"></asp:Label>
                                    </td>
                                    <td> <%--BARATHonkeyup="myFunCaps(this.id)" onblur="getcompany1(this.value)"--%>
                                        <asp:TextBox ID="txt_pername" runat="server" CssClass="textbox txtheight5" onchange="return checknamecomp(this.value)"  onfocus="return myFunction(this)" onkeydown = "return (event.keyCode!=13);" ></asp:TextBox>
                                         <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender73" runat="server" TargetControlID="txt_pername"
                                            FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .">
                                        </asp:FilteredTextBoxExtender>
                                          <asp:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="getcompnameper" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_pername"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                       
                                          <span id="rnomsgcomp"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_cname" Text="Company Name" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_cname" runat="server"  onfocus="return myFunction(this)" CssClass="textbox txtheight5 textbox1"
                                            onkeydown = "return (event.keyCode!=13);">
                                        </asp:TextBox>
                                         <%--onblur="getcompany(this.value)"--%>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender32" runat="server" TargetControlID="txt_cname"
                                            FilterType="UppercaseLetters,LowercaseLetters,custom,numbers" ValidChars=" .&@-/*">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:AutoCompleteExtender ID="autocompany" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="getcompname" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_cname"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                          <span style="color: Red;">*</span>
                                    </td>
                                </tr>
                               
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_des" Text="Designation" runat="server" ></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_des" runat="server" CssClass="textbox" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                                         <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender100" runat="server" TargetControlID="txt_des"
                                            FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_com_add" Text="Address" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_com_add" runat="server" CssClass="textbox txtheight5" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                                          <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender101" runat="server" TargetControlID="txt_com_add"
                                            FilterType="UppercaseLetters,LowercaseLetters,Numbers,custom" ValidChars="-,">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_com_str" Text="Street" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_com_str" runat="server" CssClass="textbox" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                                          <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender102" runat="server" TargetControlID="txt_com_str"
                                            FilterType="UppercaseLetters,LowercaseLetters,custom,numbers" ValidChars="-,">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_com_city" Text="City" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_com_city" runat="server" CssClass="textbox" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                                           <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender103" runat="server" TargetControlID="txt_com_city"
                                            FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars="">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                 <tr>
                                    <td>
                                        <asp:Label ID="lbl_com_pin" Text="Pin Code" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_com_pin" runat="server" CssClass="textbox" MaxLength="6" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                                           <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender104" runat="server" TargetControlID="txt_com_pin"
                                            FilterType="numbers" ValidChars="">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_com_state" Text="State" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_com_state" runat="server" CssClass="textbox" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                                           <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender105" runat="server" TargetControlID="txt_com_state"
                                            FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars="">
                                        </asp:FilteredTextBoxExtender>
                                          <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="getstate" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_com_state"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_com_county" Text="Country" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_com_county" runat="server" CssClass="textbox" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                                          <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender106" runat="server" TargetControlID="txt_com_county"
                                            FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars="">
                                        </asp:FilteredTextBoxExtender>
                                          <asp:AutoCompleteExtender ID="AutoCompleteExtender10" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="getcountry" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_com_county"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_com_ph" Text="Phone No" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_com_ph" runat="server" CssClass="textbox" MaxLength="12" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                                           <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender107" runat="server" TargetControlID="txt_com_ph"
                                            FilterType="numbers" ValidChars="">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_com_mail" Text="Mail ID" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_com_mail" runat="server" CssClass="textboxchng txtheight5" onfocus="return myFunction(this)" onblur="return checkEmail(this)" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div style="float: left; margin-left: 95px;">
                            <table>
                                <tr>
                                    <td colspan="2" align="center">
                                     <asp:Label ID="lbl_chng_from_comp" ForeColor="#008000" Font-Bold="true" Text="From" runat="server" style="display:none;" onfocus="return myFunction(this)" ></asp:Label>
                                        <asp:Label ID="lbl_com_to" ForeColor="#008000" Font-Bold="true" Text="To" runat="server" onfocus="return myFunction(this)"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:CheckBox ID="cb_com_dept" runat="server" Text="Department" onfocus="return myFunction(this)"
                                            onchange="return comp_checkchange1(this.value);" />
                                        <asp:CheckBox ID="cb_com_indi" runat="server" Text="Individual" onfocus="return myFunction(this)"
                                            onchange="return comp_checkchange2(this.value);" />
                                    </td>
                                </tr>
                            </table>
                            <div id="div_comp_dept" runat="server" style="display: none" onfocus="return myFunction(this)">
                              <span id="deptmsg3"></span>
                                 <span id="indimsg3"></span>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_comdepto" Text="Department" runat="server"></asp:Label>
                                        </td>

                                        <td>
                                            <asp:TextBox ID="txt_comdepto" runat="server" Width="190px" onchange="return checkdepartment3(this.value)" onkeyup="return checkdepartment3(this.value)" onfocus="return myFunction(this)" Height="20px" CssClass="textbox1 textbox" onkeydown = "return (event.keyCode!=13);" ></asp:TextBox>
                                       
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" TargetControlID="txt_comdepto"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="autocompany1" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getdept" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_comdepto"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <asp:Button ID="btn_comadd" runat="server" CssClass="textbox textbox1 btn1" Text="Add"
                                                OnClientClick="return comp_change();" />
                                                <asp:Button ID="btn_cm_depto_rmv" Width="58px" runat="server" CssClass="textbox textbox1 btn1" Text="Remove"
                                                OnClientClick="return comp_change0();" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_comdepto1" runat="server" onchange="return checkdepartment3(this.value)" onkeyup="return checkdepartment3(this.value)" Width="190px" Height="20px" CssClass="textbox1 textbox"
                                                Style="display: none;" onfocus="return myFunction(this)"  onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server" TargetControlID="txt_comdepto1"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="autocompany2" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getdept" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_comdepto1"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_comcc" Text="Cc" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_comcc" runat="server" Width="190px" onchange="return checkdepartment3(this.value)" onkeyup="return checkdepartment3(this.value)" onfocus="return myFunction(this)" Height="20px" CssClass="textbox1 textbox" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" TargetControlID="txt_comcc"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="autocompany3" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getdept" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_comcc"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <asp:Button ID="btn_comadd1" runat="server" CssClass="textbox textbox1 btn1" Text="Add"
                                                OnClientClick="return comp_change1();" />
                                                 <asp:Button ID="btn_cmp_deptcc_rmv" Width="58px" runat="server" CssClass="textbox textbox1 btn1" Text="Remove"
                                                OnClientClick="return comp_change11();" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_comcc2" runat="server" onchange="return checkdepartment3(this.value)" onkeyup="return checkdepartment3(this.value)" Width="190px" Height="20px" CssClass="textbox1 textbox"
                                                Style="display: none;" onfocus="return myFunction(this)" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" runat="server" TargetControlID="txt_comcc2"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="autocomany4" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getdept" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_comcc2"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="div_comp_indi" runat="server" style="display: none;margin-left:11px" onfocus="return myFunction(this)">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_com_ind" Text="Individual" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_com_ind" runat="server" Width="190px" onfocus="return myFunction(this)" Height="20px" onchange="return checkindiv3(this.value)" onkeyup="return checkindiv3(this.value)" CssClass="textbox1 textbox"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender41" runat="server" TargetControlID="txt_com_ind"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom,Numbers" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="autocomindi1" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getstaff" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_com_ind"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <asp:Button ID="btn_com_add2" runat="server" CssClass="textbox textbox1 btn1" Text="Add"
                                                OnClientClick="return comp_change2();" />
                                                 <asp:Button ID="btn_cm_indito_rmv" Width="58px" runat="server" CssClass="textbox textbox1 btn1" Text="Remove"
                                                OnClientClick="return comp_change22();" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_com_ind1" runat="server" Width="190px" Height="20px" onchange="return checkindiv3(this.value)" onkeyup="return checkindiv3(this.value)" CssClass="textbox1 textbox"
                                                Style="display: none;" onfocus="return myFunction(this)"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender42" runat="server" TargetControlID="txt_com_ind1"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom,Numbers" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="autocomindi2" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getstaff" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_com_ind1"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_com_cc" Text="Cc" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_com_cc" runat="server" Width="190px" Height="20px" onchange="return checkindiv3(this.value)" onkeyup="return checkindiv3(this.value)" onfocus="return myFunction(this)" CssClass="textbox1 textbox"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender43" runat="server" TargetControlID="txt_com_cc"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom,Numbers" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="autocomindi3" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getstaff" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_com_cc"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <asp:Button ID="btn_com_add3" runat="server" CssClass="textbox textbox1 btn1" Text="Add"
                                                OnClientClick="return comp_change3();" />
                                                 <asp:Button ID="btn_am_indicc_rmv" Width="58px" runat="server" CssClass="textbox textbox1 btn1" Text="Remove"
                                                OnClientClick="return comp_change33();" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_com_cc1" runat="server" Width="190px" Height="20px" onchange="return checkindiv3(this.value)" onkeyup="return checkindiv3(this.value)" CssClass="textbox1 textbox"
                                                Style="display: none;" onfocus="return myFunction(this)"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender44" runat="server" TargetControlID="txt_com_cc1"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom,Numbers" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="autocomindi4" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getstaff" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_com_cc1"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                    <div style="float: left; margin-left: 140px;">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_com_sub" Text="Subject" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_com_sub" runat="server" onfocus="return myFunction(this)" MaxLength="100"  Width="700px"  CssClass="textboxchng textbox1 txtheight6" onkeydown = "return (event.keyCode!=13);"> </asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender110" runat="server" TargetControlID="txt_com_sub"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom,Numbers" ValidChars=" .-&@/*">

                                            </asp:FilteredTextBoxExtender>
                                         <span style="color: Red;">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_com_doc" Text="Document Priority" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_com_doc" runat="server" CssClass="textbox  ddlheight4"
                                        Style="float: left;" onchange="changecompany1(this)" onfocus="return myFunction(this)">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txt_com_doc1" runat="server" CssClass="textbox txtheight4 textbox1"
                                        Style="display: none; float: left;" onfocus="return myFunction(this)" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender67" runat="server" TargetControlID="txt_com_doc1"
                                                FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_com_delivry" Text="Received Mode" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_com_delivry" runat="server" CssClass="textbox  ddlheight4"
                                        Style="float: left;" onchange="changecompany2(this)" onfocus="return myFunction(this)">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txt_com_deliv1" runat="server" CssClass="textbox txtheight4 textbox1"
                                        Style="display: none; float: left;" onfocus="return myFunction(this)"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_com_refno" Text="Ref No" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_com_refno" runat="server" CssClass="textbox txtheight1 textbox1" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender78" runat="server" TargetControlID="txt_com_refno"
                                                 FilterType="Custom,UppercaseLetters,LowercaseLetters,numbers" ValidChars=" .#$*&/-@">
                                            </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_com_atch" Text="Attachments" runat="server"></asp:Label> <%--delsi--%>
                                </td>
                               <%-- <td>
                                    <asp:FileUpload ID="FileUpload_com" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" />
                                    <asp:Button ID="btn_com_more" runat="server" CssClass="textbox textbox1 btn2" Text="More" OnClientClick="return browse_change();"
                                        OnClick="btn_com_more_Click" Visible="false" />
                                </td>--%>
                                <td align="left">
                                
                    
                                <div id="div_certificate_Grid" runat="server" >
                                <center>
                                  <span style="color: White; font-weight: bold;"></span>
                                  </center>
                                            <asp:GridView ID="Certificate_Grid" runat="server" AutoGenerateColumns="false" Visible="true">
                                                <Columns>
                                                                                            
                                                    <asp:TemplateField HeaderText="No of Files" HeaderStyle-ForeColor="White"  HeaderStyle-BackColor="Brown"  HeaderStyle-HorizontalAlign="center">
                                                        <ItemTemplate>
                                                          
                                                            <asp:Label ID="lbl_certificate" ReadOnly="true" runat="server" Text='<%#Eval("Files") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Browse"  HeaderStyle-ForeColor="White" HeaderStyle-BackColor="Brown" HeaderStyle-HorizontalAlign="center">
                                                        <ItemTemplate>
                                                        <asp:Label ID="lbl_mandatory" runat="server" Text="*"></asp:Label>
                                                            <asp:FileUpload ID="file_uplod" runat="server" />
                                                            
                                                        </ItemTemplate>
                                                       



                                                    </asp:TemplateField>
                       


                                                </Columns>
                                            </asp:GridView>
                                        </div> 
                                      
                                 <asp:Button ID="btn_addrows" runat="server" CssClass="textbox textbox1 type" BackColor="Brown"
                                    ForeColor="White"  Visible="true" Width="81px" Height="26px" Text="Add Rows" OnClick="btnaddrows_Click"/>
                                       
                                </td>
                               
                            
                            </tr>
                             <tr>
                            <td>
                           
                         <asp:Label ID="lbl_chargescomp" runat="server" Text="Charges"></asp:Label>
                         </td>
                         <td>
                         <asp:TextBox ID="txt_chargecomp" runat="server" CssClass="textbox textbox1 txtheight1" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                          <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender117" runat="server" TargetControlID="txt_chargecomp"
                                            FilterType="numbers" ValidChars="">
                                        </asp:FilteredTextBoxExtender></td>
                            </tr>
                             <tr>
                                        <td>
                                        <asp:Label ID="lbl_paymentcomp" Text="Payment" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                        <asp:CheckBox ID="cb_paymentcomp" runat="server" onchange="return comppayment(this.value)" onfocus="return myFunction(this)"  /></td>
                            </tr>
                        </table>
                         <div id="div_comp_payment" runat="server" style="display:none" class="maindivstyle">
                             <table ><tr>
                            <td>
                          
                            <asp:RadioButton ID="rdo_comp_cheque" runat="server" Checked="true" GroupName="pay3" Text="Cheque" onchange="return chequechangecomp(this.value)" onfocus="return myFunction(this)"  />
                           
                            <asp:RadioButton ID="rdo_comp_chedd" runat="server" GroupName="pay3" Text="DD" onchange="return ddchangecomp(this.value)" onfocus="return myFunction(this)"  />
                            <asp:Label ID="lbl_comp_bank" Text="Bank" runat="server"></asp:Label>
                            </td>
                            <td>
                           

                            <asp:DropDownList ID="txt_bank_comp" runat="server" CssClass="textbox  ddlheight4"
                                         onchange="changebankcomp(this)" onfocus="return myFunction(this)"></asp:DropDownList>
                                         </td>
                                         <td>
                                         
                            <asp:TextBox ID="txt_bank_company" runat="server" CssClass="textbox txtheight4 textbox1" Style="display: none;
                                        " onfocus="return myFunction(this)"></asp:TextBox>
                                        </td>
                                        <td>
                                
                            <asp:Label ID="lbl_comp_bankbranch" Text="Branch" runat="server"></asp:Label>
                            <asp:TextBox ID="txt_comp_bankbranch" runat="server" CssClass="textbox textbox1 txtheight1" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                          
                         
                        </td></tr></table>
                         </div>
                        <br />
                            <div id="div_comp_cheque" runat="server" style="display:none" class="maindivstyle">
                            <table>
                            <tr>
                            <td><asp:Label ID="lbl_comp_chqname" runat="server" Text="Name"></asp:Label></td>
                            <td><asp:TextBox ID="txt_comp_chqname" runat="server" CssClass="txtheight4 textbox textbox1"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender138" runat="server" TargetControlID="txt_comp_chqname"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender></td>
                             <td><asp:Label ID="lbl_comp_chqno" runat="server" Text="Cheque No"></asp:Label></td>
                            <td><asp:TextBox ID="txt_comp_chqno" runat="server" CssClass="txtheight textbox textbox1"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender139" runat="server" TargetControlID="txt_comp_chqno"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom,numbers" ValidChars=" ">
                                </asp:FilteredTextBoxExtender></td>
                             <td><asp:Label ID="lbl_comp_chqdate" runat="server" Text="Date"></asp:Label></td>
                            <td><asp:TextBox ID="txt_comp_chqdate" runat="server" CssClass="txtheight textbox textbox1"></asp:TextBox>
                             <asp:CalendarExtender ID="CalendarExtender12" TargetControlID="txt_comp_chqdate" runat="server"
                                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender></td>
                             <td><asp:Label ID="lbl_comp_chqamt" runat="server" Text="Amount"></asp:Label></td>
                            <td><asp:TextBox ID="txt_comp_chqamt" runat="server" CssClass="txtheight2 textbox textbox1"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender140" runat="server" TargetControlID="txt_comp_chqamt"
                                    FilterType="Numbers" ValidChars="">
                                </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            </table>
                            </div>
                              <div id="div_comp_dd" runat="server" style="display:none" class="maindivstyle">
                            <table>
                            <tr>
                            <td><asp:Label ID="lbl_compddname" runat="server" Text="Name"></asp:Label></td>
                            <td><asp:TextBox ID="txt_compddname" runat="server" CssClass="txtheight4 textbox textbox1"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender141" runat="server" TargetControlID="txt_compddname"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender></td>
                             <td><asp:Label ID="lbl_compddno" runat="server" Text="DD No"></asp:Label></td>
                            <td><asp:TextBox ID="txt_compddno" runat="server" CssClass="txtheight textbox textbox1"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender142" runat="server" TargetControlID="txt_compddno"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom,numbers" ValidChars=" ">
                                </asp:FilteredTextBoxExtender></td>
                             <td><asp:Label ID="lbl_comp_dddate" runat="server" Text="Date"></asp:Label></td>
                            <td><asp:TextBox ID="txt_comp_dddate" runat="server" CssClass="txtheight textbox textbox1"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender13" TargetControlID="txt_comp_dddate" runat="server"
                                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender></td>
                             <td><asp:Label ID="lbl_comp_ddamt" runat="server" Text="Amount"></asp:Label></td>
                            <td><asp:TextBox ID="txt_comp_ddamt" runat="server" CssClass="txtheight2 textbox textbox1"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender143" runat="server" TargetControlID="txt_comp_ddamt"
                                    FilterType="Numbers" ValidChars="">
                                </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            </table>
                            </div>
                    </div>
                    <div id="div7" runat="server" style="float: left; margin-top:33px; margin-left: 400px" onfocus="return myFunction(this)">
                        <asp:Button ID="btncompany_save" runat="server" CssClass="textbox textbox1 btn2" Text="Save" OnClientClick="return validcomp();" 
                            OnClick="btncompany_save_Click" />
                        <asp:Button ID="btncompany_clear" runat="server" CssClass="textbox textbox1 btn2" Text="Clear" OnClientClick="return div_company_clear();"
                            OnClick="btncompany_clear_Click" /></div>
                </div>
                <%------end of company-----%>
                <div id="div_magazine" runat="server" style="display: none;">
                    <br />
                    <center><span Style="font-size: x-large; color: #008000;">Magazine</span></center>
                    <center>
                        <table width="900px">
                            <tr>
                                <td align="right">
                                 <div class="maindivstyle" align="center" style="border-radius: 7px; width: 120px;
                                    height: 30px;">
                                    <asp:RadioButton ID="rb_magz_in" runat="server" Text="In" GroupName="c1" Checked="true" onchange="return save_mag_clear1();" />
                                    
                                    <asp:RadioButton ID="rb_magz_out" runat="server" Text="Out" GroupName="c1" onchange="return save_mag_clear();" />
                                    </div>
                                </td>
                                <td style="width:100"></td>
                                <td style="width:450px"></td>

                                <td>
                                 <div class="maindivstyle" align="center" style="border-radius: 7px; width: 360px;
                                    height: 40px;">
                                    <table><tr>
                                <td>
                                    <asp:Label ID="lbl_magzdate" Text="Date" runat="server"></asp:Label>
                                </td>
                                <td>
                                 <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                        <ContentTemplate>
                                    <asp:TextBox ID="txt_magzdate" runat="server" CssClass="textbox txtheight textbox1" AutoPostBack="true" OnTextChanged="txt_magzdate_changed"></asp:TextBox>
                                      <asp:CalendarExtender ID="CalendarExtender5" TargetControlID="txt_magzdate" runat="server"
                                                CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            </ContentTemplate>
                                            </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_magztime" Text="Time" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_maghr" Width="50px" Height="25px" runat="server" CssClass="textbox textbox1">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddl_magmin" Width="50px" Height="25px" runat="server" CssClass="textbox textbox1">
                                    </asp:DropDownList>
                                   
                                    
                                    <asp:DropDownList ID="ddl_magam" Width="50px" Height="25px" runat="server"
                                        CssClass="textbox textbox1">
                                        <asp:ListItem>AM</asp:ListItem>
                                        <asp:ListItem>PM</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                </tr></table>
                                </div>
                                </td>
                            </tr>
                        </table>
                    </center>
                    <br />
                    <div style="width: 900px;">
                        <div id="magchngdiv" style="float: left;">
                            <table>
                                <tr>
                                    <td colspan="2" align="center">
                                    <asp:Label ID="lbl_chng_to_mag" ForeColor="#008000" Font-Bold="true" Text="To" runat="server" style="display:none;" onfocus="return myFunction(this)"></asp:Label>
                                        <asp:Label ID="lbl_magz_from" ForeColor="#008000" Font-Bold="true" Text="From" runat="server" onfocus="return myFunction(this)"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_magzname" Text="Name of Magazine" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_magzname"  onfocus="return myFunction(this)" onkeyup="myFunCaps(this.id)" onblur="getmagazines(this.value)" runat="server" CssClass="textboxchng txtheight5 textbox1"></asp:TextBox>
                                         <span style="color: Red;">*</span>
                                          <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender88" runat="server" TargetControlID="txt_magzname"
                                            FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                        </asp:FilteredTextBoxExtender>
                                           <asp:AutoCompleteExtender ID="AutoCompleteExtender15" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="getmagazinename" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_magzname"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_magztype" Text="Type of Magazine" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_magztype" runat="server" onkeyup="myFunCaps(this.id)" CssClass="textboxchng txtheight5 textbox1"></asp:TextBox>
                                         <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender89" runat="server" TargetControlID="txt_magztype"
                                            FilterType="UppercaseLetters,LowercaseLetters,Custom,numbers" ValidChars=" ">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_magz_add" Text="Address" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_magz_add" runat="server" onkeyup="myFunCaps(this.id)"  CssClass="textboxchng txtheight5 textbox1"></asp:TextBox>
                                         <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender90" runat="server" TargetControlID="txt_magz_add"
                                            FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ,">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_magz_str" Text="Street" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_magz_str" runat="server" onkeyup="myFunCaps(this.id)" CssClass="textboxchng  textbox1"></asp:TextBox>
                                         <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender91" runat="server" TargetControlID="txt_magz_str"
                                            FilterType="UppercaseLetters,LowercaseLetters,numbers,Custom" ValidChars=" /,">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_magz_city" Text="City" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_magz_city" runat="server" CssClass="textboxchng  textbox1"></asp:TextBox>
                                         <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender92" runat="server" TargetControlID="txt_magz_city"
                                            FilterType="UppercaseLetters,LowercaseLetters" ValidChars="">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                  <tr>
                                    <td>
                                        <asp:Label ID="lbl_mag_pin" Text="Pin Code" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_pin" runat="server" MaxLength="6" CssClass="textbox  textbox1"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender80" runat="server" TargetControlID="txt_pin"
                                            FilterType="numbers" ValidChars="">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_magz_state" Text="State" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_magz_state" runat="server" CssClass="textboxchng  textbox1"></asp:TextBox>
                                         <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender93" runat="server" TargetControlID="txt_magz_state"
                                            FilterType="UppercaseLetters,LowercaseLetters" ValidChars="">
                                        </asp:FilteredTextBoxExtender>
                                         <asp:AutoCompleteExtender ID="AutoCompleteExtender8" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getstate" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_magz_state"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_magz_county" Text="Country" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_magz_county" runat="server" CssClass="textboxchng  textbox1"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender94" runat="server" TargetControlID="txt_magz_county"
                                            FilterType="UppercaseLetters,LowercaseLetters" ValidChars="">
                                        </asp:FilteredTextBoxExtender>
                                          <asp:AutoCompleteExtender ID="AutoCompleteExtender12" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="getcountry" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_magz_county"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                </tr>
                              
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_magz_ph" Text="Phone No" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_magz_ph" runat="server" MaxLength="15" CssClass="textbox  textbox1"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender79" runat="server" TargetControlID="txt_magz_ph"
                                            FilterType="numbers" ValidChars=" +">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_magz_mail" Text="Mail ID" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_magz_mail" runat="server" onfocus="return myFunction(this)" onblur="return checkEmail(this)" CssClass="textboxchng txtheight5 textbox1"></asp:TextBox>

                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div style="float: left; margin-left: 95px;">
                            <table>
                                <tr>
                                    <td colspan="2" align="center">
                                     <asp:Label ID="lbl_chng_from_mag" ForeColor="#008000" Font-Bold="true" Text="From" runat="server" style="display:none;" onfocus="return myFunction(this)"></asp:Label>
                                        <asp:Label ID="lbl_magz_to" ForeColor="#008000" Font-Bold="true" Text="To" runat="server" onfocus="return myFunction(this)"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:CheckBox ID="cb_magz_dept" runat="server" Text="Department" onfocus="return myFunction(this)"
                                            onchange="return magz_checkchange1(this.value);" />
                                        <asp:CheckBox ID="cb_magz_indi" runat="server" Text="Individual" onfocus="return myFunction(this)"
                                            onchange="return magz_checkchange2(this.value);" />
                                    </td>
                                </tr>
                            </table>
                            <div id="div_magz_dept" runat="server" style="display: none" onfocus="return myFunction(this)">
                              <span id="deptmsg5"></span>
                               <span id="indimsg5"></span>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_magz_depto" Text="Department" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_magz_depto" runat="server" onchange="return checkdepartment5(this.value)" onkeyup="return checkdepartment5(this.value)" onfocus="return myFunction(this)" Width="180px" Height="20px" CssClass="textbox1 textbox"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender22" runat="server" TargetControlID="txt_magz_depto"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="automagz1" runat="server" DelimiterCharacters="" Enabled="True"
                                                ServiceMethod="Getdept" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                                                CompletionSetCount="10" ServicePath="" TargetControlID="txt_magz_depto" CompletionListCssClass="autocomplete_completionListElement"
                                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <asp:Button ID="btn_magzadd" runat="server" CssClass="textbox textbox1 btn1" Text="Add"
                                                OnClientClick="return magz_change();" />
                                                 <asp:Button ID="btn_mag_depto_rmv" runat="server" Width="56px" CssClass="textbox textbox1 btn1" Text="Remove"
                                                OnClientClick="return magz_change0();" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_magz_depto1" runat="server" onchange="return checkdepartment5(this.value)" onkeyup="return checkdepartment5(this.value)" onfocus="return myFunction(this)"
                                                Style="display: none;" Width="180px" Height="20px" CssClass="textbox1 textbox"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender23" runat="server" TargetControlID="txt_magz_depto1"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="automagz2" runat="server" DelimiterCharacters="" Enabled="True"
                                                ServiceMethod="Getdept" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                                                CompletionSetCount="10" ServicePath="" TargetControlID="txt_magz_depto1" CompletionListCssClass="autocomplete_completionListElement"
                                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_magz_cc" Text="Cc" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_magz_cc" runat="server" onchange="return checkdepartment5(this.value)" onkeyup="return checkdepartment5(this.value)" onfocus="return myFunction(this)" Width="180px" Height="20px" CssClass="textbox1 textbox"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender24" runat="server" TargetControlID="txt_magz_cc"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="automagz3" runat="server" DelimiterCharacters="" Enabled="True"
                                                ServiceMethod="Getdept" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                                                CompletionSetCount="10" ServicePath="" TargetControlID="txt_magz_cc" CompletionListCssClass="autocomplete_completionListElement"
                                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <asp:Button ID="btn_magzadd1" runat="server" CssClass="textbox textbox1 btn1" Text="Add"
                                                OnClientClick="return magz_change1();" />
                                                <asp:Button ID="btn_mag_depcc_rmv" runat="server" Width="56px" CssClass="textbox textbox1 btn1" Text="Remove"
                                                OnClientClick="return magz_change11();" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_magz_cc1" runat="server" onchange="return checkdepartment5(this.value)" onkeyup="return checkdepartment5(this.value)" onfocus="return myFunction(this)" Style="display: none;"
                                                Width="180px" Height="20px" CssClass="textbox1 textbox"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender25" runat="server" TargetControlID="txt_magz_cc1"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="automagz4" runat="server" DelimiterCharacters="" Enabled="True"
                                                ServiceMethod="Getdept" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                                                CompletionSetCount="10" ServicePath="" TargetControlID="txt_magz_cc1" CompletionListCssClass="autocomplete_completionListElement"
                                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="div_magz_indi" runat="server" style="display: none; margin-left:5px" onfocus="return myFunction(this)">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_magz_indi" Text="Individual" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_magz_indi" runat="server" onfocus="return myFunction(this)" onchange="return checkindiv5(this.value)" onkeyup="return checkindiv5(this.value)" Width="185px" Height="20px" CssClass="textbox1 textbox"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender37" runat="server" TargetControlID="txt_magz_indi"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom,Numbers" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="automagindi1" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getstaff" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_magz_indi"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <asp:Button ID="btn" runat="server" CssClass="textbox textbox1 btn1" Text="Add" OnClientClick="return magz_change2();" />
                                            <asp:Button ID="btn_mag_indito_rmv" Width="55px" runat="server" CssClass="textbox textbox1 btn1" Text="Remove"
                                                OnClientClick="return magz_change22();" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_magz_indi1" runat="server" onchange="return checkindiv5(this.value)" onkeyup="return checkindiv5(this.value)" onfocus="return myFunction(this)"
                                                Style="display: none;" Width="185px" Height="20px" CssClass="textbox1 textbox"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender38" runat="server" TargetControlID="txt_magz_indi1"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom,Numbers" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="automagindi2" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getstaff" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_magz_indi1"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_magz_cc2" Text="Cc" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_magz_cc2" runat="server" onchange="return checkindiv5(this.value)" onkeyup="return checkindiv5(this.value)" onfocus="return myFunction(this)" Width="185px" Height="20px" CssClass="textbox1 textbox"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender39" runat="server" TargetControlID="txt_magz_cc2"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom,Numbers" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="automagindi3" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getstaff" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_magz_cc2"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <asp:Button ID="btn_magz_add3" runat="server" CssClass="textbox textbox1 btn1" Text="Add"
                                                OnClientClick="return magz_change3();" />
                                                <asp:Button ID="btn_mag_indicc_rmv" Width="55px" runat="server" CssClass="textbox textbox1 btn1" Text="Remove"
                                                OnClientClick="return magz_change33();" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_magz_cc3" runat="server" onchange="return checkindiv5(this.value)" onkeyup="return checkindiv5(this.value)" onfocus="return myFunction(this)" Style="display: none;"
                                                Width="185px" Height="20px" CssClass="textbox1 textbox"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender40" runat="server" TargetControlID="txt_magz_cc3"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom,Numbers" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="automagindi4" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getstaff" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_magz_cc3"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                    <div style="float: left; margin-left: 140px;">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_magz_sub" Text="Subject" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_magz_sub" runat="server"  onfocus="return myFunction(this)" MaxLength="20" Width="700px" onkeyup="myFunCaps(this.id)" CssClass="textboxchng textbox1 txtheight6">
                                        </asp:TextBox>
                                          <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender112" runat="server" TargetControlID="txt_magz_sub"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom,Numbers" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                         <span style="color: Red;">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_magz_doc" Text="Document Priority" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_magz_doc" runat="server" CssClass="textbox  ddlheight4"
                                        Style="float: left;" onchange="changemag1(this)" onfocus="return myFunction(this)">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txt_magz_doc" runat="server" CssClass="textbox txtheight4 textbox1"
                                        Style="display: none; float: left;" onfocus="return myFunction(this)"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender69" runat="server" TargetControlID="txt_magz_doc"
                                                FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_magz_delivry" Text="Delivery Mode" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_magz_delivry" runat="server" CssClass="textbox  ddlheight4"
                                        Style="float: left;" onchange="changemag2(this)" onfocus="return myFunction(this)">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txt_magz_deliv1" runat="server" CssClass="textbox txtheight4 textbox1"
                                        Style="display: none; float: left;" onfocus="return myFunction(this)"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_magz_refno" Text="Ref No" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_magz_refno" runat="server" CssClass="textbox txtheight1 textbox1"></asp:TextBox>
                                     <asp:FilteredTextBoxExtender ID="flt_magz_refo" runat="server" TargetControlID="txt_magz_refno"
                                             FilterType="Custom,UppercaseLetters,LowercaseLetters,numbers" ValidChars=" .#$*&">
                                        </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_magz_atch" Text="Attachments" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:FileUpload ID="FileUpload_magz" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" />
                                    <asp:Button ID="btn_magz_more" runat="server" CssClass="textbox textbox1 btn2" Text="More" OnClientClick="return browse_change();"
                                        OnClick="btn_magz_more_Click" Visible="false" />
                                </td>
                            </tr>
                             <tr>
                            <td>
                           
                         <asp:Label ID="Label3" runat="server" Text="Charges"></asp:Label></td>
                         <td>
                         <asp:TextBox ID="TextBox1" runat="server" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                          <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender119" runat="server" TargetControlID="TextBox1"
                                            FilterType="numbers" ValidChars="">
                                        </asp:FilteredTextBoxExtender></td>
                            </tr>
                             <tr>
                                        <td>
                                        <asp:Label ID="lbl_paymentmag" Text="Payment" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                        <asp:CheckBox ID="cb_paymentmag" runat="server" onchange="return magpayment(this.value)" onfocus="return myFunction(this)"  /></td>
                            </tr>
                        </table>

                             <table ><tr>
                            <td>
                           <div id="div_mag_payment" runat="server" style="display:none" class="maindivstyle">
                          
                            <asp:RadioButton ID="rdb_mag_chwque" runat="server" Checked="true" GroupName="pay5" Text="Cheque" onchange="return chequechangemag(this.value)" onfocus="return myFunction(this)"  />
                           
                            <asp:RadioButton ID="rdb_mag_dd" runat="server" GroupName="pay5" Text="DD" onchange="return ddchangemag(this.value)" onfocus="return myFunction(this)"  />
                            <asp:Label ID="lbl_magbank" Text="Bank" runat="server"></asp:Label>
                            <asp:DropDownList ID="ddl_magbank" runat="server" CssClass="ddlheight1 textbox textbox1"></asp:DropDownList>
                            <asp:Label ID="lbl_magbankbrabch" Text="Branch" runat="server"></asp:Label>
                            <asp:TextBox ID="txt_magbankbrabch" runat="server" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                           </div>
                         
                        </td></tr></table>
                        <br />
                            <div id="div_mag_cheque" runat="server" style="display:none" class="maindivstyle">
                            <table>
                            <tr>
                            <td><asp:Label ID="lbl_magchqname" runat="server" Text="Name"></asp:Label></td>
                            <td><asp:TextBox ID="txt_magchqname" runat="server" CssClass="txtheight4 textbox textbox1"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender150" runat="server" TargetControlID="txt_magchqname"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender></td>
                             <td><asp:Label ID="lbl_magchno" runat="server" Text="Cheque No"></asp:Label></td>
                            <td><asp:TextBox ID="txt_magchno" runat="server" CssClass="txtheight textbox textbox1"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender151" runat="server" TargetControlID="txt_magchno"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom,numbers" ValidChars=" ">
                                </asp:FilteredTextBoxExtender></td>
                             <td><asp:Label ID="lbl_mag_chqdate" runat="server" Text="Date"></asp:Label></td>
                            <td><asp:TextBox ID="txt_mag_chqdate" runat="server" CssClass="txtheight textbox textbox1"></asp:TextBox>
                             <asp:CalendarExtender ID="CalendarExtender16" TargetControlID="txt_mag_chqdate" runat="server"
                                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender></td>
                             <td><asp:Label ID="lbl_mag_chqamt" runat="server" Text="Amount"></asp:Label></td>
                            <td><asp:TextBox ID="txt_mag_chqamt" runat="server" CssClass="txtheight2 textbox textbox1"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender152" runat="server" TargetControlID="txt_mag_chqamt"
                                    FilterType="Numbers" ValidChars="">
                                </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            </table>
                            </div>
                              <div id="div_mag_dd" runat="server" style="display:none" class="maindivstyle">
                            <table>
                            <tr>
                            <td><asp:Label ID="lbl_magddname" runat="server" Text="Name"></asp:Label></td>
                            <td><asp:TextBox ID="txt_magddname" runat="server" CssClass="txtheight4 textbox textbox1"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender153" runat="server" TargetControlID="txt_magddname"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender></td>
                             <td><asp:Label ID="lbl_magddno" runat="server" Text="DD No"></asp:Label></td>
                            <td><asp:TextBox ID="txt_magddno" runat="server" CssClass="txtheight textbox textbox1"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender154" runat="server" TargetControlID="txt_magddno"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom,numbers" ValidChars=" ">
                                </asp:FilteredTextBoxExtender></td>
                             <td><asp:Label ID="lbl_magdd_date" runat="server" Text="Date"></asp:Label></td>
                            <td><asp:TextBox ID="txt_magdd_date" runat="server" CssClass="txtheight textbox textbox1"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender17" TargetControlID="txt_magdd_date" runat="server"
                                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender></td>
                             <td><asp:Label ID="lbl_magddamt" runat="server" Text="Amount"></asp:Label></td>
                            <td><asp:TextBox ID="txt_magddamt" runat="server" CssClass="txtheight2 textbox textbox1"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender155" runat="server" TargetControlID="txt_magddamt"
                                    FilterType="Numbers" ValidChars="">
                                </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            </table>
                            </div>
                    </div>
                    <div id="div4" runat="server" style="float: left; margin-top:33px; margin-left: 400px" onfocus="return myFunction(this)">
                        <asp:Button ID="btnmag_save" runat="server" CssClass="textbox textbox1 btn2" Text="Save" OnClientClick="return validmag()" OnClick="btnmag_save_Click" />
                        <asp:Button ID="btnmag_clear" runat="server" CssClass="textbox textbox1 btn2" Text="Clear" OnClientClick="return div_mag_clear();"
                            OnClick="btnmag_clear_Click" /></div>
                </div>
                <%-----end of magazine----%>
                <div id="div_others" runat="server" style="display: none;">
                    <br />
                    <center><span Style="font-size: x-large; color: #008000;">Others</span></center>
                    <center>
                        <table width="900px">
                            <tr>
                                <td align="right">
                                <div class="maindivstyle" align="center" style="border-radius: 7px; width: 120px;
                                    height: 30px;">
                                    <asp:RadioButton ID="rb_othr_in" runat="server" Text="In" GroupName="c2" Checked="true" onchange="return save_others_clear1();" />
                                   
                                    <asp:RadioButton ID="rb_othr_out" runat="server" Text="Out" GroupName="c2" onchange="return save_others_clear();" />
                                </td>

                                <td style="width:100"></td>
                                <td style="width:450px"></td>

                                <td>
                                <div class="maindivstyle" align="center" style="border-radius: 7px; width: 360px;
                                    height: 40px;">
                                <table><tr>
                                <td>
                                    <asp:Label ID="lbl_othrdate" Text="Date" runat="server"></asp:Label>
                                </td>
                                <td>
                                 <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                        <ContentTemplate>
                                    <asp:TextBox ID="txt_othrdate" runat="server" CssClass="textbox txtheight textbox1" AutoPostBack="true" OnTextChanged="txt_othrdate_changed"></asp:TextBox>
                                      <asp:CalendarExtender ID="CalendarExtender6" TargetControlID="txt_othrdate" runat="server"
                                                CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            </ContentTemplate>
                                            </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_othrtime" Text="Time" runat="server"></asp:Label>
                                </td>
                                <td>
                                   <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                        <ContentTemplate>
                                    <asp:DropDownList ID="ddl_othrhr" Width="50px" Height="25px" runat="server" CssClass="textbox textbox1">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddl_othrmin" Width="50px" Height="25px" runat="server" CssClass="textbox textbox1">
                                    </asp:DropDownList>
                                   
                                    
                                    <asp:DropDownList ID="ddl_othram" Width="50px" Height="25px" runat="server"
                                        CssClass="textbox textbox1">
                                        <asp:ListItem>AM</asp:ListItem>
                                        <asp:ListItem>PM</asp:ListItem>
                                    </asp:DropDownList>
                                    </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                </tr></table>
                                </div>
                                </td>
                            </tr>
                        </table>
                    </center>
                    <br />
                    <div style="width: 900px;">
                        <div  id="othrchngdiv" style="float: left;">
                            <table>
                                <tr>
                                    <td colspan="2" align="center">
                                     <asp:Label ID="lbl_chng_to_othr" ForeColor="#008000" Font-Bold="true" Text="To" runat="server" style="display:none;" onfocus="return myFunction(this)"></asp:Label>
                                        <asp:Label ID="lbl_othr_from" ForeColor="#008000" Font-Bold="true" Text="From" runat="server" onfocus="return myFunction(this)"></asp:Label>
                                    </td>
                                </tr>
                                 <tr>
                                    <td>
                                        <asp:Label ID="lbl_othr_pname" Text="Person Name" runat="server"></asp:Label>
                                    </td>
                                    <%--onblur="getothers1(this.value)"--%>
                                    <td>
                                        <asp:TextBox ID="txt_othr_pname" runat="server"  CssClass="textboxchng txtheight5 textbox1"  onfocus="return myFunction(this)" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="flt_oth_pname" runat="server" TargetControlID="txt_othr_pname"
                                            FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .">
                                        </asp:FilteredTextBoxExtender>
                                           <asp:AutoCompleteExtender ID="AutoCompleteExtender17" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="getotherpername" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_othr_pname"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                         <span style="color: Red;">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_othr_name" Text="Company Name" runat="server"></asp:Label>
                                    </td>
                                    <%--onblur="getothers(this.value)delssref"--%>
                                    <td>
                                        <asp:TextBox ID="txt_othr_name" runat="server"  onfocus="return myFunction(this)"  CssClass="textboxchng txtheight5 textbox1" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                                       <asp:FilteredTextBoxExtender ID="flt_oth_name" runat="server" TargetControlID="txt_othr_name"
                                            FilterType="UppercaseLetters,LowercaseLetters,custom,numbers" ValidChars=" .&@-/*">
                                           
                                        </asp:FilteredTextBoxExtender>
                                           <asp:AutoCompleteExtender ID="AutoCompleteExtender16" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="getothername" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_othr_name"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                         <span style="color: Red;">*</span>
                                    </td>
                                </tr>
                               
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_othr_add" Text="Address" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_othr_add" runat="server" CssClass="textboxchng txtheight5  textbox1" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                                         <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender95" runat="server" TargetControlID="txt_othr_add"
                                            FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=", ">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_othr_str" Text="Street" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_othr_str" runat="server"  CssClass="textboxchng  textbox1" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender96" runat="server" TargetControlID="txt_othr_str"
                                            FilterType="UppercaseLetters,LowercaseLetters,numbers,Custom" ValidChars=", /">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_othr_city" Text="City" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_othr_city" runat="server"  CssClass="textboxchng  textbox1" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender97" runat="server" TargetControlID="txt_othr_city"
                                            FilterType="UppercaseLetters,LowercaseLetters" ValidChars="">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                 <tr>
                                    <td>
                                        <asp:Label ID="lbl_ot_pin" Text="Pin Code" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_othr_pin" runat="server" MaxLength="6" CssClass="textboxchng  textbox1" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender81" runat="server" TargetControlID="txt_othr_pin"
                                            FilterType="numbers" ValidChars="">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_othr_state" Text="State" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_othr_state" runat="server" CssClass="textboxchng  textbox1" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                                         <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender98" runat="server" TargetControlID="txt_othr_state"
                                            FilterType="UppercaseLetters,LowercaseLetters" ValidChars="">
                                        </asp:FilteredTextBoxExtender>
                                          <asp:AutoCompleteExtender ID="AutoCompleteExtender9" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getstate" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_othr_state"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_othr_county" Text="Country" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_othr_county" runat="server" CssClass="textboxchng  textbox1" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                                          <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender99" runat="server" TargetControlID="txt_othr_county"
                                            FilterType="UppercaseLetters,LowercaseLetters" ValidChars="">
                                        </asp:FilteredTextBoxExtender>
                                         <asp:AutoCompleteExtender ID="AutoCompleteExtender13" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="getcountry" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_othr_county"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                       
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_othr_ph" Text="Phone No" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_othr_ph" runat="server" CssClass="textboxchng  textbox1" MaxLength="13" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                                         <asp:FilteredTextBoxExtender ID="flt_phn" runat="server" TargetControlID="txt_othr_ph"
                                            FilterType="numbers,custom" ValidChars="+ ">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_othr_mail" Text="Mail ID" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_othr_mail" runat="server" onfocus="return myFunction(this)" onblur="return checkEmail(this)" CssClass="textboxchng txtheight5 textbox1" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div style="float: left; margin-left: 95px;">
                            <table>
                                <tr>
                                    <td colspan="2" align="center">
                                     <asp:Label ID="lbl_chng_from_othr" ForeColor="#008000" Font-Bold="true" Text="From" runat="server" style="display:none;" onfocus="return myFunction(this)" ></asp:Label>
                                        <asp:Label ID="lbl_othr_to" ForeColor="#008000" Font-Bold="true" Text="To" runat="server" onfocus="return myFunction(this)"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:CheckBox ID="cb_othr_dept" runat="server" Text="Department" onfocus="return myFunction(this)"
                                            onchange="return other_checkchange1(this.value);" />
                                        <asp:CheckBox ID="cb_othr_indi" runat="server" Text="Individual" onfocus="return myFunction(this)"
                                            onchange="return other_checkchange2(this.value);" />
                                    </td>
                                </tr>
                            </table>
                            <div id="div_other_dept" runat="server" style="display: none" onfocus="return myFunction(this)">
                              <span id="deptmsg6"></span>
                                <span id="indimsg6"></span>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_othr_depto" Text="Department" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_othr_depto" runat="server" Width="190px"  onfocus="return myFunction(this)" Height="20px" onchange="return checkdepartment6(this.value)" onkeyup="return checkdepartment6(this.value)" CssClass="textbox1 textbox" onkeydown = "return (event.keyCode!=13);" ></asp:TextBox>
                                       
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender26" runat="server" TargetControlID="txt_othr_depto"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="autoother1" runat="server" DelimiterCharacters="" Enabled="True"
                                                ServiceMethod="Getdept" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                                                CompletionSetCount="10" ServicePath="" TargetControlID="txt_othr_depto" CompletionListCssClass="autocomplete_completionListElement"
                                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <asp:Button ID="btn_othr_add" runat="server" CssClass="textbox textbox1 btn1" Text="Add"
                                                OnClientClick="return other_change();" />
                                                 <asp:Button ID="btn_othr_depto_rmv" Width="58px" runat="server" CssClass="textbox textbox1 btn1" Text="Remove"
                                                OnClientClick="return other_change0();" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_othr_depto1" runat="server" onchange="return checkdepartment6(this.value)" onkeyup="return checkdepartment6(this.value)" onfocus="return myFunction(this)"
                                                Style="display: none;" Width="190px" Height="20px" CssClass="textbox1 textbox" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender27" runat="server" TargetControlID="txt_othr_depto1"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="autoother2" runat="server" DelimiterCharacters="" Enabled="True"
                                                ServiceMethod="Getdept" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                                                CompletionSetCount="10" ServicePath="" TargetControlID="txt_othr_depto1" CompletionListCssClass="autocomplete_completionListElement"
                                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_othr_cc" Text="Cc" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_othr_cc" runat="server" Width="190px" onchange="return checkdepartment6(this.value)" onkeyup="return checkdepartment6(this.value)" onfocus="return myFunction(this)" Height="20px" CssClass="textbox1 textbox" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender29" runat="server" TargetControlID="txt_othr_cc"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="autoother3" runat="server" DelimiterCharacters="" Enabled="True"
                                                ServiceMethod="Getdept" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                                                CompletionSetCount="10" ServicePath="" TargetControlID="txt_othr_cc" CompletionListCssClass="autocomplete_completionListElement"
                                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <asp:Button ID="btn_othradd1" runat="server" CssClass="textbox textbox1 btn1" Text="Add"
                                                OnClientClick="return other_change1();" />
                                                 <asp:Button ID="btn_othr_depcc_rmv" Width="58px" runat="server" CssClass="textbox textbox1 btn1" Text="Remove"
                                                OnClientClick="return other_change11();" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_othr_cc1" runat="server" onchange="return checkdepartment6(this.value)" onkeyup="return checkdepartment6(this.value)" onfocus="return myFunction(this)" Style="display: none;"
                                                Width="190px" Height="20px" CssClass="textbox1 textbox" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender28" runat="server" TargetControlID="txt_othr_cc1"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="autoother4" runat="server" DelimiterCharacters="" Enabled="True"
                                                ServiceMethod="Getdept" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                                                CompletionSetCount="10" ServicePath="" TargetControlID="txt_othr_cc1" CompletionListCssClass="autocomplete_completionListElement"
                                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="div_other_indi" runat="server" style="display: none;margin-left:10px" onfocus="return myFunction(this)">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_othr_indi" Text="Individual" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_othr_indi" runat="server" Width="190px" onfocus="return myFunction(this)" onchange="return checkindiv6(this.value)" onkeyup="return checkindiv6(this.value)" Height="20px" CssClass="textbox1 textbox"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender33" runat="server" TargetControlID="txt_othr_indi"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom,Numbers" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="autootherindiv1" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getstaff" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_othr_indi"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <asp:Button ID="btn_othr_add2" runat="server" CssClass="textbox textbox1 btn1" Text="Add"
                                                OnClientClick="return other_change2();" />
                                                 <asp:Button ID="btn_othr_indto_rmv" Width="58px" runat="server" CssClass="textbox textbox1 btn1" Text="Remove"
                                                OnClientClick="return other_change22();" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_othr_indi1" runat="server" onchange="return checkindiv6(this.value)" onkeyup="return checkindiv6(this.value)" onfocus="return myFunction(this)"
                                                Style="display: none;" Width="190px" Height="20px" CssClass="textbox1 textbox"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender34" runat="server" TargetControlID="txt_othr_indi1"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom,Numbers" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="autootherindi2" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getstaff" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_othr_indi1"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_othr_cc2" Text="Cc" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_othr_cc2" runat="server" Width="190px" onchange="return checkindiv6(this.value)" onkeyup="return checkindiv6(this.value)" onfocus="return myFunction(this)" Height="20px" CssClass="textbox1 textbox"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender35" runat="server" TargetControlID="txt_othr_cc2"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom,Numbers" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="autootherindi3" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getstaff" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_othr_cc2"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <asp:Button ID="btn_othr_add3" runat="server" CssClass="textbox textbox1 btn1" Text="Add"
                                                OnClientClick="return other_change3();" />
                                                 <asp:Button ID="btn_othr_indcc_rmv" Width="58px" runat="server" CssClass="textbox textbox1 btn1" Text="Remove"
                                                OnClientClick="return other_change33();" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_othr_cc3" runat="server" Width="190px" Height="20px" onchange="return checkindiv6(this.value)" onkeyup="return checkindiv6(this.value)" CssClass="textbox1 textbox"
                                                onfocus="return myFunction(this)" Style="display: none;">
                                            </asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender36" runat="server" TargetControlID="txt_othr_cc3"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom,Numbers" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="autootherindi4" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getstaff" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_othr_cc3"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                    <div style="float: left; margin-left: 140px;">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_othr_sub" Text="Subject" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_othr_sub" runat="server"  onfocus="return myFunction(this)" onkeydown = "return (event.keyCode!=13);" CssClass="textbox1 textboxchng txtheight6" Width="700px" >
                                        </asp:TextBox>
                                          <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender113" runat="server" TargetControlID="txt_othr_sub"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom,Numbers" ValidChars=" .-&@/*">
                                            </asp:FilteredTextBoxExtender>
                                         <span style="color: Red;">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_othr_doc" Text="Document Priority" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_othr_doc" runat="server" CssClass="textbox  ddlheight4"
                                        Style="float: left;" onchange="changeother1(this)" onfocus="return myFunction(this)">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txt_othr_doc" runat="server" CssClass="textbox txtheight4 textbox1"
                                        Style="display: none; float: left;" onfocus="return myFunction(this)" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                                         <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender70" runat="server" TargetControlID="txt_othr_doc"
                                                FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" -">
                                            </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_othr_delivry" Text="Received Mode" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_othr_delivry" runat="server" CssClass="textbox  ddlheight4"
                                        Style="float: left;" onchange="changeother2(this)" onfocus="return myFunction(this)">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txt_othr_delivry" runat="server" CssClass="textbox txtheight4 textbox1"
                                        Style="display: none; float: left;" onfocus="return myFunction(this)" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_othr_refno" Text="Ref No" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_othr_refno" runat="server" CssClass="textbox txtheight1 textbox1" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                                    
                                     <asp:FilteredTextBoxExtender ID="flt_othr_refno" runat="server" TargetControlID="txt_othr_refno"
                                            FilterType="Custom,UppercaseLetters,LowercaseLetters,numbers" ValidChars=" .#$*&/-@">
                                        </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_othr_atch" Text="Attachments" runat="server"></asp:Label>
                                </td>
                                <%--<td>
                                    <asp:FileUpload ID="FileUpload_others" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" />
                                    <asp:Button ID="btn_othr_more" runat="server" CssClass="textbox textbox1 btn2" Text="More" OnClientClick="return browse_change();"
                                        OnClick="btn_othr_more_Click" Visible="false" />
                                </td>--%>


                                <td>

                                 <div id="divGrid_others" runat="server" >
                                <center>
                                  <span style="color: White; font-weight: bold;"></span>
                                  </center>
                                            <asp:GridView ID="Grid_others" runat="server" AutoGenerateColumns="false" Visible="true">
                                                <Columns>
                                                                                            
                                                    <asp:TemplateField HeaderText="No of Files" HeaderStyle-ForeColor="White"  HeaderStyle-BackColor="Brown"  

HeaderStyle-HorizontalAlign="center">
                                                        <ItemTemplate>
                                                          
                                                            <asp:Label ID="lbl_certificate3" ReadOnly="true" runat="server" Text='<%#Eval("Files") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Browse"  HeaderStyle-ForeColor="White" HeaderStyle-BackColor="Brown" HeaderStyle-HorizontalAlign="center">
                                                        <ItemTemplate>
                                                        <asp:Label ID="lbl_mandatory3" runat="server" Text="*"></asp:Label>
                                                            <asp:FileUpload ID="file_uplod3" runat="server" />
                                                            
                                                        </ItemTemplate>
                                                       



                                                    </asp:TemplateField>
                       


                                                </Columns>
                                            </asp:GridView>
                                        </div> 
                                      
                                        <asp:Button ID="others_addrow" runat="server" CssClass="textbox textbox1 type" BackColor="Brown"
                                    ForeColor="White"  Visible="true" Width="81px" Height="26px" Text="Add Rows"
                                     OnClick="btnaddrows2_Click" />
                                 
                                </td>


                            </tr>
                             <tr>
                            <td>
                         <asp:Label ID="lbl_chargesothr" runat="server" Text="Charges"></asp:Label>
                         </td>
                         <td>
                         <asp:TextBox ID="txt_othersothr" runat="server" CssClass="textbox textbox1 txtheight1" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                          <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender120" runat="server" TargetControlID="txt_othersothr"
                                            FilterType="numbers" ValidChars="">
                                        </asp:FilteredTextBoxExtender></td>
                            </tr>
                             <tr>
                                        <td>
                                        <asp:Label ID="lbl_paymentothr" Text="Payment" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                        <asp:CheckBox ID="cb_paymentothr" runat="server" onchange="return otherpayment(this.value)" onfocus="return myFunction(this)"  /></td>
                            </tr>
                            
                        </table>
                         <div id="div_othr_payment" runat="server" style="display:none" class="maindivstyle">
                             <table ><tr>
                            <td>
                          
                          
                            <asp:RadioButton ID="rdo_othr_cheque" runat="server" Checked="true" GroupName="pay6" Text="Cheque" onchange="return chequechangeothr(this.value)" onfocus="return myFunction(this)"  />
                           
                            <asp:RadioButton ID="rdo_othr_dd" runat="server" GroupName="pay6" Text="DD" onchange="return ddchangeothr(this.value)" onfocus="return myFunction(this)"  />
                            <asp:Label ID="lbl_othr_bank" Text="Bank" runat="server"></asp:Label>
                            </td>
                            <td>

                            
                          

                            <asp:DropDownList ID="ddl_othr_bank" runat="server" CssClass="textbox  ddlheight4"
                                         onchange="changebankothers(this)" onfocus="return myFunction(this)"></asp:DropDownList>
                                         </td>
                                         <td>
                                         
                            <asp:TextBox ID="txt_others_bank" runat="server" CssClass="textbox txtheight4 textbox1" Style="display: none;
                                        " onfocus="return myFunction(this)"></asp:TextBox>
                                        </td>
                                        <td>
                                      

                            <asp:Label ID="lbl_othr_banknranch" Text="Branch" runat="server"></asp:Label>
                            <asp:TextBox ID="txt_othr_banknranch" runat="server" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                           
                         
                        </td></tr></table>
                        </div>
                        <br />
                            <div id="div_othr_cheque" runat="server" style="display:none" class="maindivstyle">
                            <table>
                            <tr>
                            <td><asp:Label ID="lbl_othr_chqname" runat="server" Text="Name"></asp:Label></td>
                            <td><asp:TextBox ID="txt_othr_chqname" runat="server" CssClass="txtheight4 textbox textbox1" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender156" runat="server" TargetControlID="txt_othr_chqname"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender></td>
                             <td><asp:Label ID="lbl_othr_chqno" runat="server" Text="Cheque No"></asp:Label></td>
                            <td><asp:TextBox ID="txt_othr_chqno" runat="server" CssClass="txtheight textbox textbox1" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender157" runat="server" TargetControlID="txt_othr_chqno"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom,numbers" ValidChars=" ">
                                </asp:FilteredTextBoxExtender></td>
                             <td><asp:Label ID="lbl_othr_chqdate" runat="server" Text="Date"></asp:Label></td>
                            <td><asp:TextBox ID="txt_othr_chqdate" runat="server" CssClass="txtheight textbox textbox1" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                             <asp:CalendarExtender ID="CalendarExtender18" TargetControlID="txt_othr_chqdate" runat="server"
                                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender></td>
                             <td><asp:Label ID="lbl_othr_chqamt" runat="server" Text="Amount"></asp:Label></td>
                            <td><asp:TextBox ID="txt_othr_chqamt" runat="server" CssClass="txtheight2 textbox textbox1" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender158" runat="server" TargetControlID="txt_othr_chqamt"
                                    FilterType="Numbers" ValidChars="">
                                </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            </table>
                            </div>
                              <div id="div_othr_dd" runat="server" style="display:none" class="maindivstyle">
                            <table>
                            <tr>
                            <td><asp:Label ID="lbl_othr_ddname" runat="server" Text="Name"></asp:Label></td>
                            <td><asp:TextBox ID="txt_othr_ddname" runat="server" CssClass="txtheight4 textbox textbox1"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender159" runat="server" TargetControlID="txt_othr_ddname"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender></td>
                             <td><asp:Label ID="lbl_othr_ddno" runat="server" Text="DD No"></asp:Label></td>
                            <td><asp:TextBox ID="txt_othr_ddno" runat="server" CssClass="txtheight textbox textbox1"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender160" runat="server" TargetControlID="txt_othr_ddno"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom,numbers" ValidChars=" ">
                                </asp:FilteredTextBoxExtender></td>
                             <td><asp:Label ID="lbl_othr_dddate" runat="server" Text="Date"></asp:Label></td>
                            <td><asp:TextBox ID="txt_othr_dddate" runat="server" CssClass="txtheight textbox textbox1"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender19" TargetControlID="txt_othr_dddate" runat="server"
                                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender></td>
                             <td><asp:Label ID="lbl_othr_ddamt" runat="server" Text="Amount"></asp:Label></td>
                            <td><asp:TextBox ID="txt_othr_ddamt" runat="server" CssClass="txtheight2 textbox textbox1"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender161" runat="server" TargetControlID="txt_othr_ddamt"
                                    FilterType="Numbers" ValidChars="">
                                </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            </table>
                            </div>
                    </div>
                    <div id="div5" runat="server" style="float: left; margin-top:33px; margin-left: 400px" onfocus="return myFunction(this)">
                        <asp:Button ID="btnother_save" runat="server" CssClass="textbox textbox1 btn2" Text="Save" OnClientClick="return validothr()" 
                            OnClick="btnother_save_Click" />
                        <asp:Button ID="btnother_clear" runat="server" CssClass="textbox textbox1 btn2" Text="Clear" OnClientClick="return div_others_clear();"
                            OnClick="btnother_clear_Click" /></div>
                </div>
                <%----end of other----%>
                <div id="div_individu" runat="server" style="display: none;">
                    <br />
                    <center><span Style="font-size: x-large; color: #008000;">Institution</span></center>
                    <center>
                        <table width="900px">
                            <tr>
                                <td align="right">
                                 <div class="maindivstyle" align="center" style="border-radius: 7px; width: 120px;
                                    height: 30px;">
                                    <asp:RadioButton ID="rb_indiv_in" runat="server" Text="In" GroupName="c3" Checked="true" onchange="return save_indi_clear1();" />
                                    
                                    <asp:RadioButton ID="rb_indiv_out" runat="server" Text="Out" GroupName="c3" onchange="return save_indi_clear();" />
                                    </div>
                                </td>
                                <td style="width:100"></td>
                                <td style="width:450px"></td>

                                <td>
                                <div class="maindivstyle" align="center" style="border-radius: 7px; width: 360px;
                                    height: 40px;">
                                <table><tr>
                                <td>
                                    <asp:Label ID="lbl_ind_date" Text="Date" runat="server"></asp:Label>
                                </td>
                                <td>
                                 <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                        <ContentTemplate>
                                    <asp:TextBox ID="txt_ind_date" runat="server" CssClass="textbox txtheight textbox1" AutoPostBack="true" OnTextChanged="txt_ind_date_changed"></asp:TextBox>
                                      <asp:CalendarExtender ID="CalendarExtender4" TargetControlID="txt_ind_date" runat="server"
                                                CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            </ContentTemplate>
                                            </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_ind_time" Text="Time" runat="server"></asp:Label>
                                </td>
                                <td>
                                 <asp:DropDownList ID="ddl_indhr" Width="50px" Height="25px" runat="server" CssClass="textbox textbox1">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddlindmin" Width="50px" Height="25px" runat="server" CssClass="textbox textbox1">
                                    </asp:DropDownList>
                                   
                                    
                                    <asp:DropDownList ID="ddlinam" Width="50px" Height="25px" runat="server"
                                        CssClass="textbox textbox1">
                                        <asp:ListItem>AM</asp:ListItem>
                                        <asp:ListItem>PM</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                </tr></table>
                                </div>
                                </td>
                            </tr>
                        </table>
                    </center>
                    <br />
                    <div style="width: 900px;">
                        <div id="indichngdiv" style="float: left;">
                            <table>
                                <tr>
                                    <td colspan="2" align="center">
                                     <asp:Label ID="lbl_chng_to_indi" ForeColor="#008000" Font-Bold="true" Text="To" runat="server" style="display:none;" onfocus="return myFunction(this)"></asp:Label>
                                        <asp:Label ID="lbl_ind_from" ForeColor="#008000" Font-Bold="true" Text="From" runat="server" onfocus="return myFunction(this)"></asp:Label>
                                    </td>
                                </tr>
                                   <tr>
                                    <td>
                                        <asp:Label ID="lbl_ind_cname" Text="Institution Name" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_ind_cname" runat="server" onkeyup="myFunCaps(this.id)" onfocus="return myFunction(this)" CssClass="textbox txtheight5 textbox1" onblur="getindividual(this.value)"></asp:TextBox>
                                         <asp:FilteredTextBoxExtender ID="flt_ind_cname" runat="server" TargetControlID="txt_ind_cname"
                                            FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .">
                                        </asp:FilteredTextBoxExtender>
                                           <asp:AutoCompleteExtender ID="AutoCompleteExtender14" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="getindiname" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_ind_cname"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                         <span style="color: Red;">*</span>
                                    </td>
                                </tr>
                                   <tr>
                                    <td>
                                        <asp:Label ID="lbl_ind_pname" Text="Person Name" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_ind_pname" onkeyup="myFunCaps(this.id)" runat="server" CssClass="textboxchng txtheight5 textbox1"></asp:TextBox>
                                         <asp:FilteredTextBoxExtender ID="flt_ind_pname" runat="server" TargetControlID="txt_ind_pname"
                                            FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .">
                                        </asp:FilteredTextBoxExtender>
                                        
                                    </td>
                                </tr>
                             
                             
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_ind_des" Text="Designation" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_ind_des" runat="server" onkeyup="myFunCaps(this.id)" CssClass="textboxchng  textbox1"></asp:TextBox>
                                         <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender87" runat="server" TargetControlID="txt_ind_des"
                                            FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_ind_add" Text="Address" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_ind_add" runat="server" onkeyup="myFunCaps(this.id)" CssClass="textboxchng txtheight5 textbox1"></asp:TextBox>
                                         <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender86" runat="server" TargetControlID="txt_ind_add"
                                            FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ,/">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_ind_str" Text="Street" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_ind_str" runat="server" onkeyup="myFunCaps(this.id)" CssClass="textboxchng  textbox1"></asp:TextBox>
                                         <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender85" runat="server" TargetControlID="txt_ind_str"
                                            FilterType="UppercaseLetters,LowercaseLetters,custom,numbers" ValidChars=" ,/">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_ind_city" Text="City" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_ind_city" onkeyup="myFunCaps(this.id)" runat="server" CssClass="textboxchng  textbox1"></asp:TextBox>
                                         <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender82" runat="server" TargetControlID="txt_ind_city"
                                            FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                 <tr>
                                    <td>
                                        <asp:Label ID="lbl_ind_pin" Text="Pin Code" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_ind_pin" runat="server" CssClass="textboxchng  textbox1" MaxLength="6"></asp:TextBox>
                                         <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender71" runat="server" TargetControlID="txt_ind_pin"
                                            FilterType="numbers" ValidChars="">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_ind_state" Text="State" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_ind_state" runat="server" CssClass="textboxchng  textbox1"></asp:TextBox>
                                         <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender83" runat="server" TargetControlID="txt_ind_state"
                                            FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars="">
                                        </asp:FilteredTextBoxExtender>
                                         <asp:AutoCompleteExtender ID="AutoCompleteExtender7" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getstate" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_ind_state"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                          
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_ind_county" Text="Country" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_ind_county" runat="server" CssClass="textboxchng  textbox1"></asp:TextBox>
                                         <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender84" runat="server" TargetControlID="txt_ind_county"
                                            FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars="">
                                        </asp:FilteredTextBoxExtender>
                                          <asp:AutoCompleteExtender ID="AutoCompleteExtender11" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="getcountry" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_ind_county"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_ind_ph" Text="Phone No" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_ind_ph" runat="server" CssClass="textboxchng  textbox1" MaxLength="12"></asp:TextBox>
                                         <asp:FilteredTextBoxExtender ID="flt_ind_phn" runat="server" TargetControlID="txt_ind_ph"
                                            FilterType="Custom,numbers" ValidChars="+ ">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                 
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_ind_mail" Text="Mail ID" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_ind_mail" runat="server" CssClass="textboxchng txtheight5 textbox1" onfocus="return myFunction(this)" onblur="return checkEmail(this)"></asp:TextBox>
                                       

                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div style="float: left; margin-left: 95px;">
                            <table>
                                <tr>
                                    <td colspan="2" align="center">
                                         <asp:Label ID="lbl_chng_from_indi" ForeColor="#008000" Font-Bold="true" Text="From" runat="server" style="display:none;" onfocus="return myFunction(this)" ></asp:Label>
                                        <asp:Label ID="lbl_ind_to" ForeColor="#008000" Font-Bold="true" Text="To" runat="server" onfocus="return myFunction(this)"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:CheckBox ID="cb_ind_dept" runat="server" onfocus="return myFunction(this)" onchange="return indiv_checkchange1(this.value);"
                                            Text="Department" />
                                        <asp:CheckBox ID="cb_ind_indi" runat="server" onfocus="return myFunction(this)" onchange="return indiv_checkchange2(this.value);"
                                            Text="Individual" />
                                    </td>
                                </tr>
                            </table>
                            <div id="div_indiv_dept" runat="server" style="display: none" onfocus="return myFunction(this)">
                              <span id="deptmsg4"></span>
                                 <span id="indimsg4"></span>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_ind_depto" Text="Department" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_ind_depto" runat="server" onfocus="return myFunction(this)" onchange="return checkdepartment4(this.value)" onkeyup="return checkdepartment4(this.value)" Width="190px" CssClass="textboxchng txtheight4 textbox1"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" runat="server" TargetControlID="txt_ind_depto"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom,Numbers" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="autoindi1" runat="server" DelimiterCharacters="" Enabled="True"
                                                ServiceMethod="Getdept" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                                                CompletionSetCount="10" ServicePath="" TargetControlID="txt_ind_depto" CompletionListCssClass="autocomplete_completionListElement"
                                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <asp:Button ID="btn_indadd" runat="server" CssClass="textbox textbox1 btn1" Text="Add"
                                                OnClientClick="return indiv_change();" />
                                                 <asp:Button ID="btn_indi_depto_rmv" Width="58px" runat="server" CssClass="textbox textbox1 btn1" Text="Remove"
                                                OnClientClick="return indiv_change0();" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_ind_depto1" runat="server" onchange="return checkdepartment4(this.value)" onkeyup="return checkdepartment4(this.value)" Width="190px" Style="display: none;" onfocus="return myFunction(this)"
                                                CssClass="textboxchng txtheight4 textbox1"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender19" runat="server" TargetControlID="txt_ind_depto1"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom,Numbers" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="autoindi2" runat="server" DelimiterCharacters="" Enabled="True"
                                                ServiceMethod="Getdept" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                                                CompletionSetCount="10" ServicePath="" TargetControlID="txt_ind_depto1" CompletionListCssClass="autocomplete_completionListElement"
                                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_ind_cc" Text="Cc" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_ind_cc" runat="server" Width="190px" onchange="return checkdepartment4(this.value)" onkeyup="return checkdepartment4(this.value)" onfocus="return myFunction(this)" CssClass="textboxchng txtheight4 textbox1"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender20" runat="server" TargetControlID="txt_ind_cc"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom,Numbers" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="autoindi3" runat="server" DelimiterCharacters="" Enabled="True"
                                                ServiceMethod="Getdept" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                                                CompletionSetCount="10" ServicePath="" TargetControlID="txt_ind_cc" CompletionListCssClass="autocomplete_completionListElement"
                                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <asp:Button ID="btn_indadd1" runat="server" CssClass="textbox textbox1 btn1" Text="Add"
                                                OnClientClick="return indiv_change1();" />
                                                  <asp:Button ID="btn_indi_depcc_rmv" Width="58px" runat="server" CssClass="textbox textbox1 btn1" Text="Remove"
                                                OnClientClick="return indiv_change11();" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_ind_cc1" runat="server" Width="190px" onchange="return checkdepartment4(this.value)" onkeyup="return checkdepartment4(this.value)" Style="display: none;" onfocus="return myFunction(this)"
                                                CssClass="textbox txtheight4 textbox1"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender21" runat="server" TargetControlID="txt_ind_cc1"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom,Numbers" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="autoindi4" runat="server" DelimiterCharacters="" Enabled="True"
                                                ServiceMethod="Getdept" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                                                CompletionSetCount="10" ServicePath="" TargetControlID="txt_ind_cc1" CompletionListCssClass="autocomplete_completionListElement"
                                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="div_indiv_indi" runat="server" style="display: none;margin-left:04px" onfocus="return myFunction(this)">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_ind_indivto" Text="Individual" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_ind_indivto" runat="server" Width="200px" onfocus="return myFunction(this)" onchange="return checkindiv4(this.value)" onkeyup="return checkindiv4(this.value)" CssClass="textbox txtheight4 textbox1"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender59" runat="server" TargetControlID="txt_ind_indivto"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom,Numbers" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="autoindi_indi1" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getstaff" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_ind_indivto"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <asp:Button ID="btn_ind_add2" runat="server" CssClass="textbox textbox1 btn1" Text="Add"
                                                OnClientClick="return indiv_change2();" />
                                                  <asp:Button ID="btn_indi_into_rmv" Width="58px" runat="server" CssClass="textbox textbox1 btn1" Text="Remove"
                                                OnClientClick="return indiv_change22();" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_ind_indiv1" runat="server" onchange="return checkindiv4(this.value)" onkeyup="return checkindiv4(this.value)" Width="200px" Style="display: none;" onfocus="return myFunction(this)"
                                                CssClass="textbox txtheight4 textbox1"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender60" runat="server" TargetControlID="txt_ind_indiv1"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom,Numbers" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="autoindi_indi2" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getstaff" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_ind_indiv1"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_ind_indivcc" Text="Cc" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_ind_indivcc" runat="server" Width="200px" onfocus="return myFunction(this)" onchange="return checkindiv4(this.value)" onkeyup="return checkindiv4(this.value)" CssClass="textbox txtheight4 textbox1"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender61" runat="server" TargetControlID="txt_ind_indivcc"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom,Numbers" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="autoindi_indiv3" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getstaff" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_ind_indivcc"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <asp:Button ID="btn_ind_add3" runat="server" CssClass="textbox textbox1 btn1" Text="Add"
                                                OnClientClick="return indiv_change3();" />
                                                  <asp:Button ID="btn_ind_intcc_rmv" Width="58px" runat="server" CssClass="textbox textbox1 btn1" Text="Remove"
                                                OnClientClick="return indiv_change33();" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_ind_indivcc1" runat="server" onchange="return checkindiv4(this.value)" onkeyup="return checkindiv4(this.value)" Width="200px" CssClass="textbox txtheight4 textbox1"
                                                Style="display: none;" onfocus="return myFunction(this)"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender62" runat="server" TargetControlID="txt_ind_indivcc1"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom,Numbers" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="autoindi_indi4" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getstaff" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_ind_indivcc1"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                    <div style="float: left; margin-left: 140px;">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_ind_sub" Text="Subject" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_ind_sub" runat="server"  onfocus="return myFunction(this)" MaxLength="20" CssClass="textboxchng textbox1 txtheight6" onkeyup="myFunCaps(this.id)" Width="700px"></asp:TextBox>
                                 <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender111" runat="server" TargetControlID="txt_ind_sub"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom,Numbers" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                         <span style="color: Red;">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_ind_doc" Text="Document Priority" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_ind_doc" runat="server" CssClass="textbox  ddlheight4"
                                        Style="float: left;" onchange="changeindi1(this)" onfocus="return myFunction(this)">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txt_ind_doc1" runat="server" CssClass="textbox txtheight4 textbox1"
                                        Style="display: none; float: left;" onfocus="return myFunction(this)"></asp:TextBox>
                                         <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender68" runat="server" TargetControlID="txt_ind_doc1"
                                                FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" -">
                                            </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_ind_delivry" Text="Delivery Mode" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_ind_delivry" runat="server" CssClass="textbox  ddlheight4"
                                        Style="float: left;" onchange="changeindi2(this)" onfocus="return myFunction(this)">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txt_ind_deliv1" runat="server" CssClass="textbox txtheight4 textbox1"
                                        Style="display: none; float: left;" onfocus="return myFunction(this)"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_ind_refno" Text="Ref No" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_ind_refno" runat="server" CssClass="textbox txtheight1 textbox1"></asp:TextBox>
                                     <asp:FilteredTextBoxExtender ID="lbl_ind_refo" runat="server" TargetControlID="txt_ind_refno"
                                             FilterType="Custom,UppercaseLetters,LowercaseLetters,numbers" ValidChars=" .#$*&">
                                        </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_ind_atch" Text="Attachments" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:FileUpload ID="FileUpload_ind" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" />
                                    <asp:Button ID="btn_ind_more" runat="server" CssClass="textbox textbox1 btn2" Text="More" OnClientClick="return browse_change();"
                                        OnClick="btn_com_more_Click" Visible="false" />
                                </td>
                            </tr>
                             <tr>
                            <td>
                          
                         <asp:Label ID="lbl_chargesindi" runat="server" Text="Charges"></asp:Label>
                         </td>
                         <td>
                         <asp:TextBox ID="txt_chargesindi" runat="server" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                          <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender118" runat="server" TargetControlID="txt_chargesindi"
                                            FilterType="numbers" ValidChars="">
                                        </asp:FilteredTextBoxExtender></td>
                            </tr>
                             <tr>
                                        <td>
                                        <asp:Label ID="lbl_paymentindi" Text="Payment" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                        <asp:CheckBox ID="cb_paymentindi" runat="server" onchange="return instpayment(this.value)" onfocus="return myFunction(this)"  /></td>
                            </tr>
                        </table>
                             <table ><tr>
                            <td>
                           <div id="div_indi_payment" runat="server" style="display:none" class="maindivstyle">
                          
                            <asp:RadioButton ID="rdb_indi_cheque" runat="server" Checked="true" GroupName="pay4" Text="Cheque" onchange="return chequechangeindi(this.value)" onfocus="return myFunction(this)"  />
                           
                            <asp:RadioButton ID="rdb_indi_dd" runat="server" GroupName="pay4" Text="DD" onchange="return ddchangeindi(this.value)" onfocus="return myFunction(this)"  />
                            <asp:Label ID="lbl_indi_bank" Text="Bank" runat="server"></asp:Label>
                            <asp:DropDownList ID="dd_indi_bank" runat="server" CssClass="ddlheight1 textbox textbox1"></asp:DropDownList>
                            <asp:Label ID="lbl_indi_bankbranch" Text="Branch" runat="server"></asp:Label>
                            <asp:TextBox ID="txt_indi_bankbranch" runat="server" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                           </div>
                         
                        </td></tr></table>
                        <br />
                            <div id="div_indi_cheque" runat="server" style="display:none" class="maindivstyle">
                            <table>
                            <tr>
                            <td><asp:Label ID="lbl_indi_chqname" runat="server" Text="Name"></asp:Label></td>
                            <td><asp:TextBox ID="txt_indi_chqname" runat="server" CssClass="txtheight4 textbox textbox1"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender144" runat="server" TargetControlID="txt_indi_chqname"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender></td>
                             <td><asp:Label ID="lbl_indi_chqno" runat="server" Text="Cheque No"></asp:Label></td>
                            <td><asp:TextBox ID="txt_indi_chqno" runat="server" CssClass="txtheight textbox textbox1"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender145" runat="server" TargetControlID="txt_indi_chqno"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom,numbers" ValidChars=" ">
                                </asp:FilteredTextBoxExtender></td>
                             <td><asp:Label ID="lbl_indi_chqdate" runat="server" Text="Date"></asp:Label></td>
                            <td><asp:TextBox ID="txt_indi_chqdate" runat="server" CssClass="txtheight textbox textbox1"></asp:TextBox>
                             <asp:CalendarExtender ID="CalendarExtender14" TargetControlID="txt_indi_chqdate" runat="server"
                                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender></td>
                             <td><asp:Label ID="lbl_indi_chqamt" runat="server" Text="Amount"></asp:Label></td>
                            <td><asp:TextBox ID="txt_indi_chqamt" runat="server" CssClass="txtheight2 textbox textbox1"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender146" runat="server" TargetControlID="txt_indi_chqamt"
                                    FilterType="Numbers" ValidChars="">
                                </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            </table>
                            </div>
                              <div id="div_indi_dd" runat="server" style="display:none" class="maindivstyle">
                            <table>
                            <tr>
                            <td><asp:Label ID="lbl_comddname" runat="server" Text="Name"></asp:Label></td>
                            <td><asp:TextBox ID="txt_comddname" runat="server" CssClass="txtheight4 textbox textbox1"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender147" runat="server" TargetControlID="txt_comddname"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender></td>
                             <td><asp:Label ID="lbl_indiddno" runat="server" Text="DD No"></asp:Label></td>
                            <td><asp:TextBox ID="txt_indiddno" runat="server" CssClass="txtheight textbox textbox1"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender148" runat="server" TargetControlID="txt_indiddno"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom,numbers" ValidChars=" ">
                                </asp:FilteredTextBoxExtender></td>
                             <td><asp:Label ID="lbl_indidddate" runat="server" Text="Date"></asp:Label></td>
                            <td><asp:TextBox ID="txt_indidddate" runat="server" CssClass="txtheight textbox textbox1"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender15" TargetControlID="txt_indidddate" runat="server"
                                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender></td>
                             <td><asp:Label ID="lbl_indi_dd_amt" runat="server" Text="Amount"></asp:Label></td>
                            <td><asp:TextBox ID="txt_indi_dd_amt" runat="server" CssClass="txtheight2 textbox textbox1"></asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender149" runat="server" TargetControlID="txt_indi_dd_amt"
                                    FilterType="Numbers" ValidChars="">
                                </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            </table>
                            </div>
                    </div>
                    <div id="div6" runat="server" style="float: left; margin-top:33px; margin-left: 400px" onfocus="return myFunction(this)">
                        <asp:Button ID="btnindi_save" runat="server" CssClass="textbox textbox1 btn2" Text="Save" OnClientClick="return validindi()" 
                            OnClick="btnindi_save_Click" />
                        <asp:Button ID="btnindi_clear" runat="server" CssClass="textbox textbox1 btn2" Text="Clear" OnClientClick="return div_individual_clear();"
                            OnClick="btnindi_clear_Click" /></div>
                </div>
                <%------end of individual----%>
           <%--  ------------------   payment pop----------------------------%>




                 <div id="imgdiv2" runat="server" visible="false" style="height: 350%; z-index: 1000;
        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
        left: 0px;">
        <center>
            <div id="pnl2" runat="server" class="table" style="background-color: White;
                height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                margin-top: 200px; border-radius: 10px;">
                <center>
                <table style=" height:100px; width:100%">
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblalerterr" runat="server"  style="color:Red;" Font-Bold="true"
                                Font-Size="Medium"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                    <td>
                     <center>
                     <asp:Button ID="btnerrclose" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px; " OnClientClick="return tot_alert();"
                                    OnClick="btnerrclose_Click"  Text="Ok" runat="server"  />
                                    </center>
                    </td>
                    </tr>
                 
                </table>
                </center>
            </div>
        </center>
    </div>

            </div>
    </div>
  
    </form>
</body>
</html>
</asp:Content>

