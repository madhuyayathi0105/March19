<%@ Page Title="" Language="C#" MasterPageFile="~/RequestMOD/RequestSubSiteMaster.master" AutoEventWireup="true" CodeFile="GatePassEntryExit.aspx.cs" Inherits="GatePassEntryExit" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
      <meta name="viewport" content="initial-scale=1.0;width=device-width" />
   <%-- <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>--%>
    <script type="text/javascript" src="../Scripts/jquery-1.4.1.min.js"></script>
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
    .txtcaps
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
      .maindivstylesize
        {
            height: 1300px;
            width: 1000px;
        }
   
    </style>
    <script type="text/javascript">

      function PrintDiv() {

          var panel = document.getElementById("<%=contentDiv.ClientID %>");
          var printWindow = window.open('', '', 'height=auto,width=685');
          printWindow.document.write('<html');
          printWindow.document.write('<head> <style type="text/css"> p{ font-size: x-small;margin: 0px; padding: 0px; border: 0px;  } body{ margin:0px;}</style>');
          printWindow.document.write('</head><body>');
          printWindow.document.write('<form>');
          printWindow.document.write(panel.innerHTML);
          printWindow.document.write(' </form>');
          printWindow.document.write('</body></html>');
          printWindow.document.close();
          setTimeout(function () {
              printWindow.print();
          }, 500);
          return false;
      }
function enterscript(e)
{
    if(e.keycode==13 || e.which==13)
    {
       // alert("Don't press enter key")
        return false;
    }
}
function justChack(id)
{
var myVar = setInterval(function(){ getsmart(id) }, 1000);
//clearInterval(myVar);
}
function justCheck(id)
{
var myVar = setInterval(function(){ getstaffbysmartcard(id) }, 1000);
//clearInterval(myVar);
}
function myStopFunction() {
    clearInterval(myVar);
}
    var btntype=0;
        function studinoutclr() {
            studentclear();
        }
        function staffinoutclr() {
            staffclear();
        }
        function parentinoutclr() {
            parentclear();
        }
        function visitorinoutclr() {
         var rb_individual = document.getElementById('<%=rb_individual.ClientID %>');
             var rb_in = document.getElementById('<%=rb_visitin.ClientID %>');
                var rb_out = document.getElementById('<%=rb_visitout.ClientID %>');
              var rb_company = document.getElementById('<%=rb_company.ClientID %>');
               document.getElementById('<%=txt_compname.ClientID %>').value="";
               if(rb_in.checked)
            {
            

                var tename= document.getElementById('<%=TextBox1.ClientID %>');
    var tename1= document.getElementById('<%=txt_name4.ClientID %>');
                   var tename2= document.getElementById('<%=txt_mno.ClientID %>');
                   tename1.style.backgroundColor = "White";
                   tename2.style.backgroundColor = "White";
                 tename.style.backgroundColor = "#ffffcc";
             
                  document.getElementById('<%=txt_compname.ClientID %>').style.backgroundColor= "White";
                  var tename3= document.getElementById('<%=txt_desgn.ClientID %>');
                   var tename4= document.getElementById('<%=txt_dep.ClientID %>');
                   tename3.style.backgroundColor = "White";
                   tename4.style.backgroundColor = "White";
                    var tename5= document.getElementById('<%=txt_vehtype.ClientID %>');
                   var tename6= document.getElementById('<%=txt_visit1.ClientID %>');
                   tename5.style.backgroundColor = "White";
                   tename6.style.backgroundColor = "White";
                    var tename7= document.getElementById('<%=txt_vehno1.ClientID %>');
                   var tename8= document.getEleme5ntById('<%=txt_cty.ClientID %>');
                   tename7.style.backgroundColor = "White";
                   tename8.style.backgroundColor = "White";
                    var tename9= document.getElementById('<%=txt_stat.ClientID %>');
                   var tename10= document.getElementById('<%=txt_dis.ClientID %>');
                   tename9.style.backgroundColor = "White";
                   tename10.style.backgroundColor = "White";
                    var tename11= document.getElementById('<%=txt_str.ClientID %>');
                   tename11.style.backgroundColor = "White";
                  var mage= document.getElementById("<%=hid.ClientID %>").value ;
                document.getElementById('<%=TextBox1.ClientID %>').value=mage;
                document.getElementById('<%=txt_mno.ClientID %>').value="";
                document.getElementById('<%=txt_name4.ClientID %>').value=""
                 document.getElementById('<%=txt_desgn.ClientID %>').value="";
                document.getElementById('<%=txt_dep.ClientID %>').value=""
                 document.getElementById('<%=txt_vehtype.ClientID %>').value="";
                document.getElementById('<%=txt_visit1.ClientID %>').value=""
                 document.getElementById('<%=txt_vehno1.ClientID %>').value="";
                document.getElementById('<%=txt_cty.ClientID %>').value=""
                 document.getElementById('<%=txt_stat.ClientID %>').value="";
                document.getElementById('<%=txt_dis.ClientID %>').value=""
                 document.getElementById('<%=txt_str.ClientID %>').value="";
                   document.getElementById('<%=TextBox1.ClientID %>').disabled = true;
                
            }
             if(rb_out.checked)
            {
                var tename= document.getElementById('<%=TextBox1.ClientID %>');
                 tename.style.backgroundColor = "White";
                 document.getElementById('<%=TextBox1.ClientID %>').value="";
                  var tename1= document.getElementById('<%=txt_name4.ClientID %>');
                   var tename2= document.getElementById('<%=txt_mno.ClientID %>');
                   tename1.style.backgroundColor = "#ffffcc";
                   tename2.style.backgroundColor = "#ffffcc";

                  document.getElementById('<%=TextBox1.ClientID %>').disabled=false;

                    var tename3= document.getElementById('<%=txt_desgn.ClientID %>');
                   var tename4= document.getElementById('<%=txt_dep.ClientID %>');
                   tename3.style.backgroundColor = "#ffffcc";
                   tename4.style.backgroundColor = "#ffffcc";
                    var tename5= document.getElementById('<%=txt_vehtype.ClientID %>');
                   var tename6= document.getElementById('<%=txt_visit1.ClientID %>');
                   tename5.style.backgroundColor = "#ffffcc";
                   tename6.style.backgroundColor = "#ffffcc";
                    var tename7= document.getElementById('<%=txt_vehno1.ClientID %>');
                   var tename8= document.getElementById('<%=txt_cty.ClientID %>');
                   tename7.style.backgroundColor = "#ffffcc";
                   tename8.style.backgroundColor = "#ffffcc";
                    var tename9= document.getElementById('<%=txt_stat.ClientID %>');
                   var tename10= document.getElementById('<%=txt_dis.ClientID %>');
                   tename9.style.backgroundColor = "#ffffcc";
                   tename10.style.backgroundColor = "#ffffcc";
                    var tename11= document.getElementById('<%=txt_str.ClientID %>');
                   tename11.style.backgroundColor = "#ffffcc";
                   document.getElementById('<%=txt_compname.ClientID %>').style.backgroundColor= "#ffffcc";
                
                 
                 
            }
            if(rb_individual.checked)
            {
             document.getElementById('<%=txt_compname.ClientID %>').style.display = "none";
             document.getElementById('<%=lbl_compname.ClientID %>').style.display = "none";
            }
            if(rb_company.checked)
            {
             document.getElementById('<%=txt_compname.ClientID %>').style.display = "block";
             document.getElementById('<%=lbl_compname.ClientID %>').style.display = "block";
            }

            companyclear();
        }
        function materialinoutclr() {
            materialclear();
        }
        function vehicleinoutclr() {
            vehicleclear();
        }
        function checkrno(txt1) {
            $.ajax({
                type: "POST",
                url: "GatePassEntryExit.aspx/CheckRollNo",
                data: '{Roll_No: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccessRno,
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function OnSuccessRno(response) {
            var mesg = $("#rnomsg")[0];
            switch (response.d) {
                case "0":
                    // bindData();
                    mesg.style.color = "red";
                    mesg.innerHTML = "RollNo not exist";
                    studrnoclear();
                    break;
                case "1":
                   // getrno();
                    mesg.style.color = "green";
                    mesg.innerHTML = "Available";
                    break;
                case "error":
                    mesg.style.color = "red";
                    mesg.innerHTML = "Error occurred";
                    break;
            }
        }
        function getrno(txt1) {
     var dne=  txt1.value;
  
            $.ajax({
                type: "POST",
                url: "GatePassEntryExit.aspx/getData",
                data: '{Roll_No: "' + dne + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    bindss(response.d);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function checksmartno(txt1) {
            $.ajax({
                type: "POST",
                url: "GatePassEntryExit.aspx/CheckSmNo",
                data: '{SmartNo: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccessSno,
                failure: function (responsesm) {
                    alert(responsesm);
                }
            });
        }
        function OnSuccessSno(responsesm) {
            var mesg = $("#smarterr")[0];
            switch (responsesm.d) {
                case "0":
                    // bindData();
                    mesg.style.color = "red";
                    mesg.innerHTML = "SmartNo not exist";
                    smartclear();
                    break;
                case "1":
                   // getrno();
                    mesg.style.color = "green";
                    mesg.innerHTML = "Available";
                    break;
                case "error":
                    mesg.style.color = "red";
                    mesg.innerHTML = "Error occurred";
                    break;
            }
        }
        function getsmart(txt1) {
     var dne=  txt1.value;
     var n = dne.length;
   
     if(n>=10)
     {
   
     var someVarName = "";
     var someVarName1 = localStorage.getItem("someVarName");
     
     if(someVarName1=="" || someVarName1=="Null")
     {
     localStorage.setItem("someVarName", someVarName);
     someVarName1="";
     }  
      
    
     if(someVarName1!=dne)
     {      
     localStorage.setItem("someVarName", dne);
     //var dw="1";
      var usercode = document.getElementById("<%=lblUserCode.ClientID%>");
       var dw = usercode.innerHTML;
            $.ajax({
                type: "POST",
                url: "GatePassEntryExit.aspx/studentSmartCard",
                data: '{Smart_No: "' + dne + '",j: "' + dw + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (responsesm) {
                    bindss(responsesm.d);
                },
                failure: function (responsesm) {
                    alert(responsesm);
                }
            });
            }
            else{
           
            localStorage.clear();
            }
            }
            else{
             localStorage.clear();
            }
        }
          //       rolllll nooooooooooooo....................................
function getsmartrollno(txt1) {
     var dne= document.getElementById('<%=txt_rollno.ClientID %>').value;
     document.getElementById('<%=txt_rollno.ClientID %>').value=dne;
           var usercode = document.getElementById("<%=lblUserCode.ClientID%>");

       dne =      document.getElementById('<%=txt_rollno.ClientID %>').value;
       var dw = usercode.innerHTML;
                  $.ajax({
                type: "POST",
                url: "GatePassEntryExit.aspx/studroll",
                data: '{RollNo: "' + dne + '",j: "' + dw + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (responsesm) {
                    bindss(responsesm.d);

                },
                failure: function (responsesm) {
                    alert(responsesm);

                }
            });
           
        }

////      
////        
////                
function getdeptno(txt1) {
     var dne=  txt1.value; 
      var usercode = document.getElementById("<%=lblUserCode.ClientID%>");
       var dw = usercode.innerHTML;
                  $.ajax({
                type: "POST",
                url: "GatePassEntryExit.aspx/studrolls",
                data: '{deptt: "' + dne + '",j: "' + dw + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (responsesms) {
                    bindsnma(responsesms.d);
                },
                failure: function (responsesms) {
                    alert(responsesms);
                }
            });
           
        }

        
          //magesh 9.6.18
           function getsmartr(txt1) {
           var name = document.getElementById('<%=Txtde.ClientID %>').value;
           $.ajax({
                type: "POST",
                url: "GatePassEntryExit.aspx/ProcessIT",
                data: '{names: "' + name + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccessSno,
                failure: function (responsesm) {
                    alert(responsesm);
                }
            });
      
      
           
//            $.ajax({
//                type: "POST",
//                url: "GatePassEntryExit.aspx/checkdepot",
//                data: '{des: "' + txt1 + '"}',
//                contentType: "application/json; charset=utf-8",
//                dataType: "json",
//                success: OnSuccessSno,
//                failure: function (responsesm) {
//                    alert(responsesm);
//                }
//            });
        }
        
               function bindsnma(Employees) {

//             var Smartno=Employees[0].smartno;
//             document.getElementById('<%=txt_smart.ClientID %>').value= "";
                      
                var rno = Employees[0].RollNo;
                document.getElementById('<%=txt_rollno.ClientID %>').value = rno;

//                var name = Employees[0].Name;
//                document.getElementById('<%=txt_name.ClientID %>').value = name;
var stu_dept= Employees[0].studept;
 document.getElementById('<%=Txtde.ClientID %>').value = stu_dept;
                var stud_type = Employees[0].Student_Type;
                document.getElementById('<%=txt_studtype.ClientID %>').value = stud_type;
                var degree = Employees[0].Degree;
                document.getElementById('<%=txt_degree.ClientID %>').value = degree;
                var dept = Employees[0].Department;
                document.getElementById('<%=txt_dept.ClientID %>').value = dept;
                var sem = Employees[0].Semester;
                document.getElementById('<%=txt_sem.ClientID %>').value = sem;
                var sec = Employees[0].Section;
                document.getElementById('<%=txt_sec.ClientID %>').value = sec;
                var img = Employees[0].photo;
                document.getElementById('<%=image2.ClientID %>').src = img;
                document.getElementById('<%=image2.ClientID %>').value = img;
               
                var staffname = Employees[0].staffname;
                document.getElementById('<%=txt_apstaff.ClientID %>').value = staffname;
                var staffdesg = Employees[0].staffdesg;
                document.getElementById('<%=txt_apdesgn.ClientID %>').value = staffdesg;
                var staffdept = Employees[0].staffdept;
                document.getElementById('<%=txt_apdept.ClientID %>').value = staffdept;
                var staffimg = Employees[0].staffphoto;
                document.getElementById('<%=image4.ClientID %>').src = staffimg;
                document.getElementById('<%=image4.ClientID %>').value = staffimg;
 
                        var visitfarphoto = Employees[0].Regvisitfarphoto;
                document.getElementById('<%=imageregvisitfar.ClientID %>').src = visitfarphoto;
                document.getElementById('<%=imageregvisitfar.ClientID %>').value = visitfarphoto;

                   var visitmonphoto = Employees[0].Regvisitmonphoto;
                document.getElementById('<%=imageregvisitmon.ClientID %>').src = visitmonphoto;
                document.getElementById('<%=imageregvisitmon.ClientID %>').value = visitmonphoto;
                 var visitgaurphoto = Employees[0].Regvisitgaurphoto;
                document.getElementById('<%=imageregvisitgaur.ClientID %>').src = visitgaurphoto;
                document.getElementById('<%=imageregvisitgaur.ClientID %>').value = visitgaurphoto;
       
        }


        function getsmartstudname(txt1) {
     var dne=  txt1.value; 
    //var dw="1";
     var usercode = document.getElementById("<%=lblUserCode.ClientID%>");
       var dw = usercode.innerHTML;
            $.ajax({
                type: "POST",
                url: "GatePassEntryExit.aspx/studname",
                data: '{Name: "' + dne + '",j: "' + dw + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (responsesm) {
                    bindss(responsesm.d);
                },
                failure: function (responsesm) {
                    alert(responsesm);
                }
            });
           
        }
        function getname(txt1) {
            $.ajax({
                type: "POST",
                url: "GatePassEntryExit.aspx/getData1",
                data: '{Name: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    bindss(response.d);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function bindss(Employees) {

            var statusmsg1 = Employees[0].statusmsg;
            if (statusmsg1 == "0") {
        
             var Smartno=Employees[0].smartno;
             document.getElementById('<%=txt_smart.ClientID %>').value= "";
              var stu_dept= Employees[0].studept;
 document.getElementById('<%=Txtde.ClientID %>').value = stu_dept;
                var rno = Employees[0].RollNo;
             
                document.getElementById('<%=txt_rollno.ClientID %>').value = rno;
                var name = Employees[0].Name;
             
                document.getElementById('<%=txt_name.ClientID %>').value = name;
                var stud_type = Employees[0].Student_Type;
            
                document.getElementById('<%=txt_studtype.ClientID %>').value = stud_type;
                var degree = Employees[0].Degree;
                document.getElementById('<%=txt_degree.ClientID %>').value = degree;
                var dept = Employees[0].Department;
                document.getElementById('<%=txt_dept.ClientID %>').value = dept;
                var sem = Employees[0].Semester;
                document.getElementById('<%=txt_sem.ClientID %>').value = sem;
                var sec = Employees[0].Section;
                document.getElementById('<%=txt_sec.ClientID %>').value = sec;
                var img = Employees[0].photo;
                document.getElementById('<%=image2.ClientID %>').src = img;
                document.getElementById('<%=image2.ClientID %>').value = img;
                statusmsg.style.color = "green";
                statusmsg.innerHTML = "Approved";
                var staffname = Employees[0].staffname;
                document.getElementById('<%=txt_apstaff.ClientID %>').value = staffname;
                var staffdesg = Employees[0].staffdesg;
                document.getElementById('<%=txt_apdesgn.ClientID %>').value = staffdesg;
                var staffdept = Employees[0].staffdept;
                document.getElementById('<%=txt_apdept.ClientID %>').value = staffdept;
                var staffimg = Employees[0].staffphoto;
                document.getElementById('<%=image4.ClientID %>').src = staffimg;
                document.getElementById('<%=image4.ClientID %>').value = staffimg;
                var appdateExit = Employees[0].appdateExit;  
                document.getElementById('<%=txt_apdate.ClientID %>').value = appdateExit;
                var apptimeExit = Employees[0].apptimeExit;
                document.getElementById('<%=txt_aptime.ClientID %>').value = apptimeExit;
                var appdateEntry = Employees[0].appdateEntry;
                document.getElementById('<%=txt_expdate.ClientID %>').value = appdateEntry;
                var apptimeEntry = Employees[0].apptimeEntry;
                document.getElementById('<%=txt_exptime.ClientID %>').value = apptimeEntry;
                var gatepurpose = Employees[0].purpose;
                 document.getElementById('<%=txt_purpose1.ClientID %>').value = gatepurpose;
                  
                      var visitfarphoto = Employees[0].Regvisitfarphoto;
                document.getElementById('<%=imageregvisitfar.ClientID %>').src = visitfarphoto;
                document.getElementById('<%=imageregvisitfar.ClientID %>').value = visitfarphoto;
                   var visitmonphoto = Employees[0].Regvisitmonphoto;
                document.getElementById('<%=imageregvisitmon.ClientID %>').src = visitmonphoto;
                document.getElementById('<%=imageregvisitmon.ClientID %>').value = visitmonphoto;
                 var visitgaurphoto = Employees[0].Regvisitgaurphoto;
                document.getElementById('<%=imageregvisitgaur.ClientID %>').src = visitgaurphoto;
                document.getElementById('<%=imageregvisitgaur.ClientID %>').value = visitgaurphoto;
                 var checkk = Employees[0].InOut;
             
                   //magesh 31.5.18
               if(checkk=="1")
               {
                document.getElementById('<%=rb_out.ClientID %>').checked=true;
                document.getElementById('<%=rb_in.ClientID %>').checked=false;
               }
               else{
               document.getElementById('<%=rb_out.ClientID %>').checked=false;
               document.getElementById('<%=rb_in.ClientID %>').checked=true;
               }
                  
            }
            else if (statusmsg1 == "1") {
             statusmsg.style.color = "red";
                statusmsg.innerHTML = "Not Approved";
            var Smartno=Employees[0].smartno;
             document.getElementById('<%=txt_smart.ClientID %>').value= "";
              var rno = Employees[0].RollNo;
                document.getElementById('<%=txt_rollno.ClientID %>').value = rno;
                var name = Employees[0].Name;
                document.getElementById('<%=txt_name.ClientID %>').value = name;
                var stud_type = Employees[0].Student_Type;
                document.getElementById('<%=txt_studtype.ClientID %>').value = stud_type;
                var degree = Employees[0].Degree;
                document.getElementById('<%=txt_degree.ClientID %>').value = degree;
                var dept = Employees[0].Department;
                document.getElementById('<%=txt_dept.ClientID %>').value = dept;
                var sem = Employees[0].Semester;
                document.getElementById('<%=txt_sem.ClientID %>').value = sem;
                var sec = Employees[0].Section;
                document.getElementById('<%=txt_sec.ClientID %>').value = sec;
                var img = Employees[0].photo;
                document.getElementById('<%=image2.ClientID %>').src = img;
                document.getElementById('<%=image2.ClientID %>').value = img;
                 var staffname = Employees[0].staffname;
                document.getElementById('<%=txt_apstaff.ClientID %>').value = staffname;
                var staffdesg = Employees[0].staffdesg;
                document.getElementById('<%=txt_apdesgn.ClientID %>').value = staffdesg;
                var staffdept = Employees[0].staffdept;
                document.getElementById('<%=txt_apdept.ClientID %>').value = staffdept;
                var staffimg = Employees[0].staffphoto;
                document.getElementById('<%=image4.ClientID %>').src = staffimg;
                document.getElementById('<%=image4.ClientID %>').value = staffimg;

                
                     var visitfarphoto = Employees[0].Regvisitfarphoto;
                document.getElementById('<%=imageregvisitfar.ClientID %>').src = visitfarphoto;
                document.getElementById('<%=imageregvisitfar.ClientID %>').value = visitfarphoto;
                   var visitmonphoto = Employees[0].Regvisitmonphoto;
                document.getElementById('<%=imageregvisitmon.ClientID %>').src = visitmonphoto;
                document.getElementById('<%=imageregvisitmon.ClientID %>').value = visitmonphoto;
                 var visitgaurphoto = Employees[0].Regvisitgaurphoto;
                document.getElementById('<%=imageregvisitgaur.ClientID %>').src = visitgaurphoto;
                document.getElementById('<%=imageregvisitgaur.ClientID %>').value = visitgaurphoto;
                 var checkk = Employees[0].InOut;
                 //magesh 31.5.18
               if(checkk=="1")
               {
                document.getElementById('<%=rb_out.ClientID %>').checked=true;
                document.getElementById('<%=rb_in.ClientID %>').checked=false;
               }
               else{
               document.getElementById('<%=rb_out.ClientID %>').checked=false;
               document.getElementById('<%=rb_in.ClientID %>').checked=true;
               }
            }
             else if (statusmsg1 == "5")
             {
              statusmsg.innerHTML = "Student  Waiting For Approval";
               var Smartno=Employees[0].smartno;
             document.getElementById('<%=txt_smart.ClientID %>').value= "";
              var stu_dept= Employees[0].studept;
 document.getElementById('<%=Txtde.ClientID %>').value = stu_dept;
                var rno = Employees[0].RollNo;
          
                document.getElementById('<%=txt_rollno.ClientID %>').value = rno;
            
                var name = Employees[0].Name;
                document.getElementById('<%=txt_name.ClientID %>').value = name;
                var stud_type = Employees[0].Student_Type;
            
                document.getElementById('<%=txt_studtype.ClientID %>').value = stud_type;
                var degree = Employees[0].Degree;
                document.getElementById('<%=txt_degree.ClientID %>').value = degree;
                var dept = Employees[0].Department;
                document.getElementById('<%=txt_dept.ClientID %>').value = dept;
                var sem = Employees[0].Semester;
                document.getElementById('<%=txt_sem.ClientID %>').value = sem;
                var sec = Employees[0].Section;
                document.getElementById('<%=txt_sec.ClientID %>').value = sec;
                var img = Employees[0].photo;
                document.getElementById('<%=image2.ClientID %>').src = img;
                document.getElementById('<%=image2.ClientID %>').value = img;
               
                var staffname = Employees[0].staffname;
                document.getElementById('<%=txt_apstaff.ClientID %>').value = staffname;
                var staffdesg = Employees[0].staffdesg;
                document.getElementById('<%=txt_apdesgn.ClientID %>').value = staffdesg;
                var staffdept = Employees[0].staffdept;
                document.getElementById('<%=txt_apdept.ClientID %>').value = staffdept;
                var staffimg = Employees[0].staffphoto;
                document.getElementById('<%=image4.ClientID %>').src = staffimg;
                document.getElementById('<%=image4.ClientID %>').value = staffimg;
                var appdateExit = Employees[0].appdateExit;  
                document.getElementById('<%=txt_apdate.ClientID %>').value = appdateExit;
                var apptimeExit = Employees[0].apptimeExit;
                document.getElementById('<%=txt_aptime.ClientID %>').value = apptimeExit;
                var appdateEntry = Employees[0].appdateEntry;
                document.getElementById('<%=txt_expdate.ClientID %>').value = appdateEntry;
                var apptimeEntry = Employees[0].apptimeEntry;
                document.getElementById('<%=txt_exptime.ClientID %>').value = apptimeEntry;
                var gatepurpose = Employees[0].purpose;
                 document.getElementById('<%=txt_purpose1.ClientID %>').value = gatepurpose;
                  
                      var visitfarphoto = Employees[0].Regvisitfarphoto;
                document.getElementById('<%=imageregvisitfar.ClientID %>').src = visitfarphoto;
                document.getElementById('<%=imageregvisitfar.ClientID %>').value = visitfarphoto;
                   var visitmonphoto = Employees[0].Regvisitmonphoto;
                document.getElementById('<%=imageregvisitmon.ClientID %>').src = visitmonphoto;
                document.getElementById('<%=imageregvisitmon.ClientID %>').value = visitmonphoto;
                 var visitgaurphoto = Employees[0].Regvisitgaurphoto;
                document.getElementById('<%=imageregvisitgaur.ClientID %>').src = visitgaurphoto;
                document.getElementById('<%=imageregvisitgaur.ClientID %>').value = visitgaurphoto;
                 var checkk = Employees[0].InOut;
  var checkk = Employees[0].InOut;

             
                   //magesh 31.5.18
               if(checkk=="1")
               {
                document.getElementById('<%=rb_out.ClientID %>').checked=false;
                document.getElementById('<%=rb_in.ClientID %>').checked=true;
               }
               else{
               document.getElementById('<%=rb_out.ClientID %>').checked=true;
               document.getElementById('<%=rb_in.ClientID %>').checked=false;
               }
             
             statusmsg.style.color = "green";
             
              statusmsg.innerHTML = "Student  Waiting For Approval";
                
             }
            else  {
             statusmsg.style.color = "red";
             if (statusmsg1 == "3"){
                statusmsg.innerHTML = "Not Register";
                }
                if (statusmsg1 == "2")
                {
                   statusmsg.innerHTML = "Please Check Your Request";
                }
            var Smartno=Employees[0].smartno;
             document.getElementById('<%=txt_smart.ClientID %>').value= "";
              var rno = Employees[0].RollNo;
                document.getElementById('<%=txt_rollno.ClientID %>').value = rno;
                var name = Employees[0].Name;
                document.getElementById('<%=txt_name.ClientID %>').value = name;
                var stud_type = Employees[0].Student_Type;
                document.getElementById('<%=txt_studtype.ClientID %>').value = stud_type;
                var degree = Employees[0].Degree;
                document.getElementById('<%=txt_degree.ClientID %>').value = degree;
                var dept = Employees[0].Department;
                document.getElementById('<%=txt_dept.ClientID %>').value = dept;
                var sem = Employees[0].Semester;
                document.getElementById('<%=txt_sem.ClientID %>').value = sem;
                var sec = Employees[0].Section;
                document.getElementById('<%=txt_sec.ClientID %>').value = sec;
                var img = Employees[0].photo;
                document.getElementById('<%=image2.ClientID %>').src = img;
                document.getElementById('<%=image2.ClientID %>').value = img;
                 var staffname = Employees[0].staffname;
                document.getElementById('<%=txt_apstaff.ClientID %>').value = staffname;
                var staffdesg = Employees[0].staffdesg;
                document.getElementById('<%=txt_apdesgn.ClientID %>').value = staffdesg;
                var staffdept = Employees[0].staffdept;
                document.getElementById('<%=txt_apdept.ClientID %>').value = staffdept;
                var staffimg = Employees[0].staffphoto;
                document.getElementById('<%=image4.ClientID %>').src = staffimg;
                document.getElementById('<%=image4.ClientID %>').value = staffimg;
                
                     var visitfarphoto = Employees[0].Regvisitfarphoto;
                document.getElementById('<%=imageregvisitfar.ClientID %>').src = visitfarphoto;
                document.getElementById('<%=imageregvisitfar.ClientID %>').value = visitfarphoto;
                   var visitmonphoto = Employees[0].Regvisitmonphoto;
                document.getElementById('<%=imageregvisitmon.ClientID %>').src = visitmonphoto;
                document.getElementById('<%=imageregvisitmon.ClientID %>').value = visitmonphoto;
                 var visitgaurphoto = Employees[0].Regvisitgaurphoto;
                document.getElementById('<%=imageregvisitgaur.ClientID %>').src = visitgaurphoto;
                document.getElementById('<%=imageregvisitgaur.ClientID %>').value = visitgaurphoto;
                
            }
        }
        function getstaffdet(txt1) {
            $.ajax({
                type: "POST",
                url: "GatePassEntryExit.aspx/getstaffdatabyname",
                data: '{Staff_Name: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    bindstaff(response.d);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function bindstaff(Staff) {
            var Staff_Name = Staff[0].Staff_Name;
            document.getElementById('<%=txt_apstaff.ClientID %>').value = Staff_Name;
            var Designation = Staff[0].Designation;
            document.getElementById('<%=txt_apdesgn.ClientID %>').value = Designation;
            var Department = Staff[0].Department;
            document.getElementById('<%=txt_apdept.ClientID %>').value = Department;
        }
        function getdriverdet(txt1) {
            $.ajax({
                type: "POST",
                url: "GatePassEntryExit.aspx/getdriverdata",
                data: '{Veh_ID: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    binddriver(response.d);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function binddriver(Driver) {
            var name = Driver[0].Name;
            document.getElementById('<%=txt_drivername.ClientID %>').value = name;
            var Veh_ID = Driver[0].Veh_ID;
            document.getElementById('<%=txt_vehicleno.ClientID %>').value = Veh_ID;
            var Mobile_No = Driver[0].Mobile_No;
            document.getElementById('<%=txt_mobile.ClientID %>').value = Mobile_No;
            var Route = Driver[0].Route;
            document.getElementById('<%=txt_route.ClientID %>').value = Route;
        }
        function checkmain() {

            var newid = document.getElementById('<%=div_student.ClientID %>');
            newid.style.display = "block";
               
 
            var staff = document.getElementById('<%=div_staff.ClientID %>');
            staff.style.display = "none";
            var parents = document.getElementById('<%=div_parent.ClientID %>');
            parents.style.display = "none";
            var visitor = document.getElementById('<%=div_visitor.ClientID %>');
            visitor.style.display = "none";
            var material = document.getElementById('<%=div_material.ClientID %>');
            material.style.display = "none";
            var vehicle = document.getElementById('<%=div_vehicle.ClientID %>');
            vehicle.style.display = "none";
            document.getElementById('<%=studenttd.ClientID %>').style.backgroundColor = "#c4c4c4";
            document.getElementById('<%=stafftd.ClientID %>').style.backgroundColor =  "#FFFFFF";
            document.getElementById('<%=parenttd.ClientID %>').style.backgroundColor =  "#FFFFFF";
            document.getElementById('<%=visitortd.ClientID %>').style.backgroundColor =  "#FFFFFF";
            document.getElementById('<%=materialtd.ClientID %>').style.backgroundColor = "#FFFFFF";          
            document.getElementById('<%=vehicletd.ClientID %>').style.backgroundColor =  "#FFFFFF";
                        
            return false;
        }
        function checkrbowninst() {
            if (document.getElementById('<%=rb_inst.ClientID %>').checked == true) {
                var newid = document.getElementById('<%=div_entryexit.ClientID %>');
                newid.style.display = "block";
                var ownid = document.getElementById('<%=div_entryexitown.ClientID %>');
                ownid.style.display = "none";
                return false;
            }
            else if (document.getElementById('<%=rb_own.ClientID %>').checked == true) {
                var ownid = document.getElementById('<%=div_entryexitown.ClientID %>');
                ownid.style.display = "block";
                var newid = document.getElementById('<%=div_entryexit.ClientID %>');
                newid.style.display = "none";
                return false;
            }
            else {
                var newid = document.getElementById('<%=div_entryexit.ClientID %>');
                newid.style.display = "none";
                var ownid = document.getElementById('<%=div_entryexitown.ClientID %>');
                ownid.style.display = "none";
                return false;
            }
        }
        //----------------------------------------for staff tab----------------------------------------------
        function staffbtn() {
            btnstaffclear();
            checkstaffrbowninst();
            setFocusToTextBox();
            var newid = document.getElementById('<%=div_student.ClientID %>');
            newid.style.display = "none";
            
            var staff = document.getElementById('<%=div_staff.ClientID %>');
            staff.style.display = "block";
            var parents = document.getElementById('<%=div_parent.ClientID %>');
            parents.style.display = "none";
            var visitor = document.getElementById('<%=div_visitor.ClientID %>');
            visitor.style.display = "none";
            var material = document.getElementById('<%=div_material.ClientID %>');
            material.style.display = "none";
            var vehicle = document.getElementById('<%=div_vehicle.ClientID %>');
            vehicle.style.display = "none";
            document.getElementById('<%=studenttd.ClientID %>').style.backgroundColor = "#FFFFFF";
            document.getElementById('<%=stafftd.ClientID %>').style.backgroundColor =  "#c4c4c4";
            document.getElementById('<%=parenttd.ClientID %>').style.backgroundColor =  "#FFFFFF";
            document.getElementById('<%=visitortd.ClientID %>').style.backgroundColor =  "#FFFFFF";
            document.getElementById('<%=materialtd.ClientID %>').style.backgroundColor = "#FFFFFF";
            document.getElementById('<%=vehicletd.ClientID %>').style.backgroundColor =  "#FFFFFF";
            return false;
        }
        function checkstaffcode(txt1) {
            $.ajax({
                type: "POST",
                url: "GatePassEntryExit.aspx/CheckStaffCode",
                data: '{Staff_Code: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccessStaff,
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function OnSuccessStaff(response) {
            var mesg = $("#staffcodemsg")[0];
            switch (response.d) {
                case "0":
                    // bindData();
                    mesg.style.color = "red";
                    mesg.innerHTML = "StaffCode not exist";
                    staffcodeclear();
                    break;
                case "1":
                    getstaffdetbyid();
                    mesg.style.color = "green";
                    mesg.innerHTML = "Available";
                    break;
                case "error":
                    mesg.style.color = "red";
                    mesg.innerHTML = "Error occurred";
                    break;
            }
        }
        function getstaffdetbyid(txt1) {
            $.ajax({
                type: "POST",
                url: "GatePassEntryExit.aspx/getstaffdatabyid",
                data: '{Staff_Code: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    bindstaffdet(response.d);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function getstaffdetbyname(txt1) {
            $.ajax({
                type: "POST",
                url: "GatePassEntryExit.aspx/getstaffdatabyname",
                data: '{Staff_Name: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    bindstaffdet(response.d);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function getapplNo(txt1) {
            $.ajax({
                type: "POST",
                url: "GatePassEntryExit.aspx/visname",
                data: '{getgateno: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    bindstaffs(response.d);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function getstaffbysmartcard(txt1) {
            var dne=  txt1.value;
         
     var n = dne.length;
      if(n>=10)
     {
   
     var someVarName1 = "";
     var someVarName1 = localStorage.getItem("someVarName1");
     
     if(someVarName1=="" || someVarName1=="Null")
     {
     localStorage.setItem("someVarName1", someVarName);
     someVarName1="";
     }        
  
     if(someVarName1!=dne)
     {   
   
     localStorage.setItem("someVarName1", dne);
     var dw="1";
            $.ajax({
                type: "POST",
                url: "GatePassEntryExit.aspx/getstaffsmartcard",
                data: '{Smart_No: "' + txt1.value + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    bindstaffdet(response.d);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }
       else{
           
            localStorage.clear();
            }
            }
            else{
             localStorage.clear();
            }
        }
          function bindstaffs(Staff) {
          
          }
           
        function bindstaffdet(Staff) {
         var statusmsg1 = Staff[0].statusmsg;
         
            if (statusmsg1 == "0" || statusmsg1 == "4") {
            if(statusmsg1 == "0" )
            {
             msgapprov.style.color = "green";
                msgapprov.innerHTML = "Approved";
                }
                if(statusmsg1 == "4" )
            {
              msgapprov.style.color = "green";
                msgapprov.innerHTML = "With Out Request";
            }
            var Staff_Code = Staff[0].Staff_Code;
            document.getElementById('<%=txt_staffid.ClientID %>').value = Staff_Code;
            var Staff_Name = Staff[0].Staff_Name;
            document.getElementById('<%=txt_staffname.ClientID %>').value = Staff_Name;
            var Department = Staff[0].Department;
            document.getElementById('<%=txt_staffdept.ClientID %>').value = Department;
            var Designation = Staff[0].Designation;
            document.getElementById('<%=txt_desg.ClientID %>').value = Designation;
            var Staff_Type = Staff[0].Staff_Type;
            document.getElementById('<%=txt_staff_type.ClientID %>').value = Staff_Type;
            var exitdate=Staff[0].appdateExit;
              document.getElementById('<%=txt_staff_type.ClientID %>').value = exitdate;
              var exittime=Staff[0].apptimeExit;
              document.getElementById('<%=txt_staff_type.ClientID %>').value = exittime;
              var entrydate=Staff[0].appdateEntry;
              document.getElementById('<%=txt_staff_type.ClientID %>').value = entrydate;
              var entrytime=Staff[0].apptimeEntry;
              document.getElementById('<%=txt_staff_type.ClientID %>').value = entrytime;
            var img = Staff[0].Photo;
            document.getElementById('<%=imagestaff.ClientID %>').src = img;
            document.getElementById('<%=imagestaff.ClientID %>').value = img;
            var checkinorout=Staff[0].checkinorout;
            if(checkinorout=="1")
              {
              document.getElementById('<%=rdo_staff_in.ClientID %>').checked==true;
              document.getElementById('<%=rdo_staff_out.ClientID %>').checked==false;
             }
             else
             {
             document.getElementById('<%=rdo_staff_in.ClientID %>').checked==false;
             document.getElementById('<%=rdo_staff_out.ClientID %>').checked==true;
             }
            }
            else if(statusmsg1=="1")
            {
              msgapprov.style.color = "red";
                msgapprov.innerHTML = "Not Approved";
             var Staff_Code = Staff[0].Staff_Code;
            document.getElementById('<%=txt_staffid.ClientID %>').value = Staff_Code;
            var Staff_Name = Staff[0].Staff_Name;
            document.getElementById('<%=txt_staffname.ClientID %>').value = Staff_Name;
            var Department = Staff[0].Department;
            document.getElementById('<%=txt_staffdept.ClientID %>').value = Department;
            var Designation = Staff[0].Designation;
            document.getElementById('<%=txt_desg.ClientID %>').value = Designation;
            var Staff_Type = Staff[0].Staff_Type;
            document.getElementById('<%=txt_staff_type.ClientID %>').value = Staff_Type;
            var img = Staff[0].Photo;
            document.getElementById('<%=imagestaff.ClientID %>').src = img;
            document.getElementById('<%=imagestaff.ClientID %>').value = img;
            var checkinorout=Staff[0].checkinorout;
              if(checkinorout=="1")
              {
              document.getElementById('<%=rdo_staff_in.ClientID %>').checked==true;
              document.getElementById('<%=rdo_staff_out.ClientID %>').checked==false;
             }
             else
             {
             document.getElementById('<%=rdo_staff_in.ClientID %>').checked==false;
             document.getElementById('<%=rdo_staff_out.ClientID %>').checked==true;
             }
            }
           else if (statusmsg1 == "3")
            {
             msgapprov.style.color = "red";
                msgapprov.innerHTML = "Not Registered";
            }
            else if(statusmsg1 == "")
            {
             var Staff_Code = Staff[0].Staff_Code;
            document.getElementById('<%=txt_staffid.ClientID %>').value = Staff_Code;
            var Staff_Name = Staff[0].Staff_Name;
            document.getElementById('<%=txt_staffname.ClientID %>').value = Staff_Name;
            var Department = Staff[0].Department;
            document.getElementById('<%=txt_staffdept.ClientID %>').value = Department;
            var Designation = Staff[0].Designation;
            document.getElementById('<%=txt_desg.ClientID %>').value = Designation;
            var Staff_Type = Staff[0].Staff_Type;
            document.getElementById('<%=txt_staff_type.ClientID %>').value = Staff_Type;
            var exitdate=Staff[0].appdateExit;
              document.getElementById('<%=txt_staff_type.ClientID %>').value = exitdate;
              var exittime=Staff[0].apptimeExit;
              document.getElementById('<%=txt_staff_type.ClientID %>').value = exittime;
              var entrydate=Staff[0].appdateEntry;
              document.getElementById('<%=txt_staff_type.ClientID %>').value = entrydate;
              var entrytime=Staff[0].apptimeEntry;
              document.getElementById('<%=txt_staff_type.ClientID %>').value = entrytime;
            var img = Staff[0].Photo;
            document.getElementById('<%=imagestaff.ClientID %>').src = img;
            document.getElementById('<%=imagestaff.ClientID %>').value = img;
            }
        }
        function checkstaffrbowninst() {
        var rdo_staff_own_trans=document.getElementById('<%=rdo_staff_own_trans.ClientID %>');
        var rdo_staff_ins_trans=document.getElementById('<%=rdo_staff_ins_trans.ClientID %>');
        var rdo_staff_other_trans=document.getElementById('<%=rdo_staff_other_trans.ClientID %>');
            if (rdo_staff_own_trans.checked) {
                var newid = document.getElementById('<%=divstaffownentryexit.ClientID %>');
                newid.style.display = "block";
                var ownid = document.getElementById('<%=divstaffinsentryexit.ClientID %>');
                ownid.style.display = "none";
                staffvehicleclear();
                return false;
            }
            else if (rdo_staff_ins_trans.checked) {
                var ownid = document.getElementById('<%=divstaffinsentryexit.ClientID %>');
                ownid.style.display = "block";
                var newid = document.getElementById('<%=divstaffownentryexit.ClientID %>');
                newid.style.display = "none";
                staffvehicleclear();
                return false;
            }
            else if (rdo_staff_other_trans.checked) {
                var newid = document.getElementById('<%=divstaffinsentryexit.ClientID %>');
                newid.style.display = "none";
                var ownid = document.getElementById('<%=divstaffownentryexit.ClientID %>');
                ownid.style.display = "none";
                
                staffvehicleclear();
                return false;
            }
        }
        function getstaffdriverdet(txt1) {
            $.ajax({
                type: "POST",
                url: "GatePassEntryExit.aspx/getdriverdata",
                data: '{Veh_ID: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    bindstaffdriver(response.d);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function bindstaffdriver(Driver) {
            var name = Driver[0].Name;
            document.getElementById('<%=txt_staff_drvname.ClientID %>').value = name;
            var Veh_ID = Driver[0].Veh_ID;
            document.getElementById('<%=txt_staff_vehilno.ClientID %>').value = Veh_ID;
            var Mobile_No = Driver[0].Mobile_No;
            document.getElementById('<%=txt_staff_mob.ClientID %>').value = Mobile_No;
            var Route = Driver[0].Route;
            document.getElementById('<%=txt_staff_route.ClientID %>').value = Route;
        }
        //-------------------------------- for parents tab-----------------------------
        function parentsbtn() {
            btnparentclear();
            var newid = document.getElementById('<%=div_student.ClientID %>');
            newid.style.display = "none";
            var staff = document.getElementById('<%=div_staff.ClientID %>');
            staff.style.display = "none";
            var parents = document.getElementById('<%=div_parent.ClientID %>');
            parents.style.display = "block";
           // document.getElementById("div_parent").style.overflow = "scroll";
            var visitor = document.getElementById('<%=div_visitor.ClientID %>');
            visitor.style.display = "none";
            var material = document.getElementById('<%=div_material.ClientID %>');
            material.style.display = "none";
            var vehicle = document.getElementById('<%=div_vehicle.ClientID %>');
            vehicle.style.display = "none";
           
            checkstudadmit(); 
            checkstudadmitmeet();
            document.getElementById('<%=studenttd.ClientID %>').style.backgroundColor = "#FFFFFF";
            document.getElementById('<%=stafftd.ClientID %>').style.backgroundColor =  "#FFFFFF";
            document.getElementById('<%=parenttd.ClientID %>').style.backgroundColor =  "#c4c4c4";
            document.getElementById('<%=visitortd.ClientID %>').style.backgroundColor =  "#FFFFFF";
            document.getElementById('<%=materialtd.ClientID %>').style.backgroundColor = "#FFFFFF";
            document.getElementById('<%=vehicletd.ClientID %>').style.backgroundColor =  "#FFFFFF";
            return false;
        }
        function checkstudadmit() {
        var rb_adm_stud=document.getElementById('<%=rb_adm_stud.ClientID %>')
        var rb_notadm_stud=document.getElementById('<%=rb_notadm_stud.ClientID %>')   
            if (rb_adm_stud.checked) {
                var admit = document.getElementById('<%=div_adm_stud.ClientID %>');
                admit.style.display = "block";
                var notadmit = document.getElementById('<%=div_notadm_stud.ClientID %>');
                notadmit.style.display = "none";               
                parentclear();
            }
            else if (rb_notadm_stud.checked) {
                var notadmit = document.getElementById('<%=div_notadm_stud.ClientID %>');
                notadmit.style.display = "block";
                var admit = document.getElementById('<%=div_adm_stud.ClientID %>');
                admit.style.display = "none";
                parentclear();
            } 
            checkstudadmitmeet();
            return false;
        }
        function checkprno(txt1) {
            $.ajax({
                type: "POST",
                url: "GatePassEntryExit.aspx/CheckRollNo",
                data: '{Roll_No: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccessPrno,
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function OnSuccessPrno(response) {
            var mesg = $("#prnomsg")[0];
            switch (response.d) {
                case "0":
                    // bindData();
                    mesg.style.color = "red";
                    mesg.innerHTML = "RollNo not exist";
                    prnoclear();
                    break;
                case "1":
                    pgetrno();
                    mesg.style.color = "green";
                    mesg.innerHTML = "Available";
                    break;
                case "error":
                    mesg.style.color = "red";
                    mesg.innerHTML = "Error occurred";
                    break;
            }
        }
        function pgetrno(txt1) {
            $.ajax({
                type: "POST",
                url: "GatePassEntryExit.aspx/prnogetdata",
                data: '{Roll_No: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    bindpstud(response.d);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function pgetname(txt1) {
            $.ajax({
                type: "POST",
                url: "GatePassEntryExit.aspx/pnamegetdata",
                data: '{Name: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    bindpstud(response.d);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function getstuddetfrmfather(txt1) {
            $.ajax({
                type: "POST",
                url: "GatePassEntryExit.aspx/getdatafrmparent",
                data: '{Father_Name: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    bindpstud(response.d);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function getdetfrmmob(txt1) {
            $.ajax({
                type: "POST",
                url: "GatePassEntryExit.aspx/getdatafrmparentmob",
                data: '{Mobile_No: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    bindpstud(response.d);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function bindpstud(Employees) {
            var rno = Employees[0].RollNo;
            document.getElementById('<%=txt_stud_rollno.ClientID %>').value = rno;
            var name = Employees[0].Name;
            document.getElementById('<%=txt_studname.ClientID %>').value = name;
            var fathername = Employees[0].Father_Name;
            document.getElementById('<%=txt_fname.ClientID %>').value = fathername;
            var fathermobile = Employees[0].Father_Mobile;
            document.getElementById('<%=txt_fmobile.ClientID %>').value = fathermobile;
            var stud_type = Employees[0].Student_Type;
            document.getElementById('<%=txt_studtype1.ClientID %>').value = stud_type;
            var degree = Employees[0].Degree;
            document.getElementById('<%=txt_pdegree.ClientID %>').value = degree;
            var dept = Employees[0].Department;
            document.getElementById('<%=txt_dept1.ClientID %>').value = dept;
            var sem = Employees[0].Semester;
            document.getElementById('<%=txt_sem1.ClientID %>').value = sem;
            var sec = Employees[0].Section;
            document.getElementById('<%=txt_section.ClientID %>').value = sec;
            var studimg = Employees[0].Stud_Photo;
            document.getElementById('<%=image5.ClientID %>').src = studimg;
            document.getElementById('<%=image5.ClientID %>').value = studimg;
            var fatherimg = Employees[0].Father_Photo;
            document.getElementById('<%=image6.ClientID %>').src = fatherimg;
            document.getElementById('<%=image6.ClientID %>').value = fatherimg;
            var motherimg = Employees[0].Mother_Photo;
            document.getElementById('<%=image7.ClientID %>').src = motherimg;
            document.getElementById('<%=image7.ClientID %>').value = motherimg;

            //barath 29.03.17 12.03
             if(document.getElementById('<%=rb_parout.ClientID %>').checked)
            {
            var tomeet = Employees[0].tomeet;
             document.getElementById('<%=rb_meetstaff.ClientID %>').checked=false;
                document.getElementById('<%=rb_meetoffice.ClientID %>').checked=false;
                document.getElementById('<%=rb_meetothers.ClientID %>').checked=false;
           if(tomeet=="0")
            { 
             document.getElementById('<%=rb_meetstaff.ClientID %>').checked=true;
              var Staff_Code = Employees[0].Staff_Code;
              document.getElementById('<%=txt_meetstaffid.ClientID %>').value = Staff_Code;
                meetstaffdetbyid(Staff_Code);
            }
             if(tomeet=="1")
            { 
             document.getElementById('<%=rb_meetoffice.ClientID %>').checked=true;
              var Staff_name = Employees[0].staff_name;
             document.getElementById('<%=txt_staffname1.ClientID %>').value = Staff_name;
             getmeetoffstaffname(Staff_name);
            }
             if(tomeet=="2")
            { 
             document.getElementById('<%=rb_meetothers.ClientID %>').checked=true;
              var othername= VisitorCompany[0].othername;
                document.getElementById('<%=txt_name1.ClientID %>').value = othername;
                var Relationship= VisitorCompany[0].Relationship;
                document.getElementById('<%=txt_relation.ClientID %>').value = Relationship;
                var MobileNo= VisitorCompany[0].MobileNo;
                document.getElementById('<%=txt_moblno.ClientID %>').value = MobileNo;
            } 
            }
        }
        function parentmeetclear() {
            document.getElementById('<%=txt_staffname1.ClientID %>').value = "";
            document.getElementById('<%=txt_dpt.ClientID %>').value = "";
            document.getElementById('<%=txt_desgtn.ClientID %>').value = "";
            document.getElementById('<%=txt_meetstaffid.ClientID %>').value = "";
            document.getElementById('<%=txt_meetstaffname.ClientID %>').value = "";
            document.getElementById('<%=txt_meetstaffdept.ClientID %>').value = "";
            document.getElementById('<%=txt_meetstaffdesg.ClientID %>').value = "";
            document.getElementById('<%=txt_name1.ClientID %>').value = "";
            document.getElementById('<%=txt_relation.ClientID %>').value = "";
            document.getElementById('<%=txt_moblno.ClientID %>').value = "";
        }
        function checkstudadmitmeet() {
        var rb_meetstaff=document.getElementById('<%=rb_meetstaff.ClientID %>');
        var rb_meetoffice=document.getElementById('<%=rb_meetoffice.ClientID %>');
        var rb_meetothers=document.getElementById('<%=rb_meetothers.ClientID %>');
            if (rb_meetstaff.checked) {
                var meetstaff = document.getElementById('<%=div_meetstaff.ClientID %>');
                meetstaff.style.display = "block";
                var meetoffice = document.getElementById('<%=div_meetoffice.ClientID %>');
                meetoffice.style.display = "none";
                var meetothers = document.getElementById('<%=div_meetothers.ClientID %>');
                meetothers.style.display = "none";
                parentmeetclear();
            }
            else if (rb_meetoffice.checked) {
                var meetoffice = document.getElementById('<%=div_meetoffice.ClientID %>');
                meetoffice.style.display = "block";
                var meetstaff = document.getElementById('<%=div_meetstaff.ClientID %>');
                meetstaff.style.display = "none";
                var meetothers = document.getElementById('<%=div_meetothers.ClientID %>');
                meetothers.style.display = "none";
                parentmeetclear();
            }
            else if (rb_meetothers.checked) {
                var meetothers = document.getElementById('<%=div_meetothers.ClientID %>');
                meetothers.style.display = "block";
                var meetstaff = document.getElementById('<%=div_meetstaff.ClientID %>');
                meetstaff.style.display = "none";
                var meetoffice = document.getElementById('<%=div_meetoffice.ClientID %>');
                meetoffice.style.display = "none";
                parentmeetclear();
            }
        }
        function getmeetoffstaffname(txt1) {
            $.ajax({
                type: "POST",
                url: "GatePassEntryExit.aspx/getstaffdatabyname",
                data: '{Staff_Name: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    bindmeetoffstaffdet(response.d);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function bindmeetoffstaffdet(Staff) {
            var Staff_Name = Staff[0].Staff_Name;
            document.getElementById('<%=txt_staffname1.ClientID %>').value = Staff_Name;
            var Department = Staff[0].Department;
            document.getElementById('<%=txt_dpt.ClientID %>').value = Department;
            var Designation = Staff[0].Designation;
            document.getElementById('<%=txt_desgtn.ClientID %>').value = Designation;
        }
        function meetstaffdetbyid(txt1) {
            $.ajax({
                type: "POST",
                url: "GatePassEntryExit.aspx/getstaffdatabyid",
                data: '{Staff_Code: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    bindmeetstaffdet(response.d);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function meetstaffdetbyname(txt1) {
            $.ajax({
                type: "POST",
                url: "GatePassEntryExit.aspx/getstaffdatabyname",
                data: '{Staff_Name: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    bindmeetstaffdet(response.d);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function bindmeetstaffdet(Staff) {
            var Staff_Code = Staff[0].Staff_Code;
            document.getElementById('<%=txt_meetstaffid.ClientID %>').value = Staff_Code;
            var Staff_Name = Staff[0].Staff_Name;
            document.getElementById('<%=txt_meetstaffname.ClientID %>').value = Staff_Name;
            var Department = Staff[0].Department;
            document.getElementById('<%=txt_meetstaffdept.ClientID %>').value = Department;
            var Designation = Staff[0].Designation;
            document.getElementById('<%=txt_meetstaffdesg.ClientID %>').value = Designation;
        }
        //---------------------------------for visitor----------------------------------------
        function visitorbtn() {
            btncompanyclear();
            var visitor = document.getElementById('<%=div_visitor.ClientID %>');
            visitor.style.display = "block";
          //  document.getElementById("div_visitor").style.overflow = "scroll";
            var newid = document.getElementById('<%=div_student.ClientID %>');
            newid.style.display = "none";
            var staff = document.getElementById('<%=div_staff.ClientID %>');
            staff.style.display = "none";
            var parents = document.getElementById('<%=div_parent.ClientID %>');
            parents.style.display = "none";
            var material = document.getElementById('<%=div_material.ClientID %>');
            material.style.display = "none";
            var vehicle = document.getElementById('<%=div_vehicle.ClientID %>');
            vehicle.style.display = "none";
            visitorappoint();
            visitorvehicle();
            //magesh 7.6.18
           // visitorreturn();
            document.getElementById('<%=TextBox1.ClientID %>').style.backgroundColor = "#ffffcc";
            document.getElementById('<%=studenttd.ClientID %>').style.backgroundColor = "#FFFFFF";
            document.getElementById('<%=stafftd.ClientID %>').style.backgroundColor =  "#FFFFFF";
            document.getElementById('<%=parenttd.ClientID %>').style.backgroundColor =  "#FFFFFF";
            document.getElementById('<%=visitortd.ClientID %>').style.backgroundColor =  "#c4c4c4";
            document.getElementById('<%=materialtd.ClientID %>').style.backgroundColor = "#FFFFFF";
            document.getElementById('<%=vehicletd.ClientID %>').style.backgroundColor =  "#FFFFFF";
            return false;
        }
        function rb_indcompname() {
             var rb_individual = document.getElementById('<%=rb_individual.ClientID %>');
             var rb_in = document.getElementById('<%=rb_visitin.ClientID %>');
                var rb_out = document.getElementById('<%=rb_visitout.ClientID %>');
              var rb_company = document.getElementById('<%=rb_company.ClientID %>');
            if (rb_individual.checked) {
                cnamespan.innerHTML = "";
                var mage= document.getElementById("<%=hid.ClientID %>").value ;
                var tename= document.getElementById('<%=TextBox1.ClientID %>');
                  tename.style.backgroundColor = "#ffffcc";
                document.getElementById('<%=TextBox1.ClientID %>').Text=mage;
                var tx=document.getElementById('<%=TextBox1.ClientID %>').Text;
//                var lcname = document.getElementById('<%=lbl_compname.ClientID %>');
//                alert(lcname);
//                lcname.style.display = "none";

//              var tcname = document.getElementById('<%=txt_compname.ClientID %>');
//                alert(tcname);
//              
//              var m=document.getElementById('<%=txt_compname.ClientID %>').Text;
//              alert(m);
             document.getElementById('<%=txt_compname.ClientID %>').style.display = "none";
             document.getElementById('<%=lbl_compname.ClientID %>').style.display = "none";
              if (rb_in.checked)
            {
             tename.style.backgroundColor = "#ffffcc";
              var tename1= document.getElementById('<%=txt_name4.ClientID %>');
                   var tename2= document.getElementById('<%=txt_mno.ClientID %>');
                   tename1.style.backgroundColor = "White";
                   tename2.style.backgroundColor = "White";
                   document.getElementById('<%=TextBox1.ClientID %>').value=mage;
                    var tename3= document.getElementById('<%=txt_desgn.ClientID %>');
                   var tename4= document.getElementById('<%=txt_dep.ClientID %>');
                   tename3.style.backgroundColor = "White";
                   tename4.style.backgroundColor = "White";
                    var tename5= document.getElementById('<%=txt_vehtype.ClientID %>');
                   var tename6= document.getElementById('<%=txt_visit1.ClientID %>');
                   tename5.style.backgroundColor = "White";
                   tename6.style.backgroundColor = "White";
                    var tename7= document.getElementById('<%=txt_vehno1.ClientID %>');
                   var tename8= document.getElementById('<%=txt_cty.ClientID %>');
                   tename7.style.backgroundColor = "White";
                   tename8.style.backgroundColor = "White";
                    var tename9= document.getElementById('<%=txt_stat.ClientID %>');
                   var tename10= document.getElementById('<%=txt_dis.ClientID %>');
                   tename9.style.backgroundColor = "White";
                   tename10.style.backgroundColor = "White";
                    var tename11= document.getElementById('<%=txt_str.ClientID %>');
                   tename11.style.backgroundColor = "White";
                    document.getElementById('<%=TextBox1.ClientID %>').disabled = true;
            }
             if (rb_out.checked)
            {
             tename.style.backgroundColor = "White";
              var tename1= document.getElementById('<%=txt_name4.ClientID %>');
                   var tename2= document.getElementById('<%=txt_mno.ClientID %>');
                   tename1.style.backgroundColor = "#ffffcc";
                   tename2.style.backgroundColor = "#ffffcc";
                     document.getElementById('<%=TextBox1.ClientID %>').value="";
                      var tename3= document.getElementById('<%=txt_desgn.ClientID %>');
                   var tename4= document.getElementById('<%=txt_dep.ClientID %>');
                   tename3.style.backgroundColor = "#ffffcc";
                   tename4.style.backgroundColor = "#ffffcc";
                    var tename5= document.getElementById('<%=txt_vehtype.ClientID %>');
                   var tename6= document.getElementById('<%=txt_visit1.ClientID %>');
                   tename5.style.backgroundColor = "#ffffcc";
                   tename6.style.backgroundColor = "#ffffcc";
                    var tename7= document.getElementById('<%=txt_vehno1.ClientID %>');
                   var tename8= document.getElementById('<%=txt_cty.ClientID %>');
                   tename7.style.backgroundColor = "#ffffcc";
                   tename8.style.backgroundColor = "#ffffcc";
                    var tename9= document.getElementById('<%=txt_stat.ClientID %>');
                   var tename10= document.getElementById('<%=txt_dis.ClientID %>');
                   tename9.style.backgroundColor = "#ffffcc";
                   tename10.style.backgroundColor = "#ffffcc";
                    var tename11= document.getElementById('<%=txt_str.ClientID %>');
                   tename11.style.backgroundColor = "#ffffcc";
                    document.getElementById('<%=TextBox1.ClientID %>').disabled = false;
            }
              companyclear();
//              alert(m);

            }
          else if (rb_company.checked) {
          cnamespan.innerHTML = "*";
            visitorbtn();
                var lcname = document.getElementById('<%=lbl_compname.ClientID %>');
                lcname.style.display = "block";
                var tcname = document.getElementById('<%=txt_compname.ClientID %>');
                  var mage= document.getElementById("<%=hid.ClientID %>").value ;
                var tename= document.getElementById('<%=TextBox1.ClientID %>');
                  tename.style.backgroundColor = "#ffffcc";
                document.getElementById('<%=TextBox1.ClientID %>').Text=mage;
                var tx=document.getElementById('<%=TextBox1.ClientID %>').Text;
                tcname.style.display = "block";
                  if (rb_in.checked)
            {
             tename.style.backgroundColor = "#ffffcc";
              var tename1= document.getElementById('<%=txt_name4.ClientID %>');
                   var tename2= document.getElementById('<%=txt_mno.ClientID %>');
                   tename1.style.backgroundColor = "White";
                   tename2.style.backgroundColor = "White";
                   document.getElementById('<%=TextBox1.ClientID %>').value=mage;
                    var tename3= document.getElementById('<%=txt_desgn.ClientID %>');
                   var tename4= document.getElementById('<%=txt_dep.ClientID %>');
                   tename3.style.backgroundColor = "White";
                   tename4.style.backgroundColor = "White";
                    var tename5= document.getElementById('<%=txt_vehtype.ClientID %>');
                   var tename6= document.getElementById('<%=txt_visit1.ClientID %>');
                   tename5.style.backgroundColor = "White";
                   tename6.style.backgroundColor = "White";
                    var tename7= document.getElementById('<%=txt_vehno1.ClientID %>');
                   var tename8= document.getElementById('<%=txt_cty.ClientID %>');
                   tename7.style.backgroundColor = "White";
                   tename8.style.backgroundColor = "White";
                    var tename9= document.getElementById('<%=txt_stat.ClientID %>');
                   var tename10= document.getElementById('<%=txt_dis.ClientID %>');
                   tename9.style.backgroundColor = "White";
                   tename10.style.backgroundColor = "White";
                    var tename11= document.getElementById('<%=txt_str.ClientID %>');
                   tename11.style.backgroundColor = "White";
                    document.getElementById('<%=TextBox1.ClientID %>').disabled = true;
            }
             if (rb_out.checked)
            {
             tename.style.backgroundColor = "White";
              var tename1= document.getElementById('<%=txt_name4.ClientID %>');
                   var tename2= document.getElementById('<%=txt_mno.ClientID %>');
                   tename1.style.backgroundColor = "#ffffcc";
                   tename2.style.backgroundColor = "#ffffcc";
                     document.getElementById('<%=TextBox1.ClientID %>').value="";
                      var tename3= document.getElementById('<%=txt_desgn.ClientID %>');
                   var tename4= document.getElementById('<%=txt_dep.ClientID %>');
                   tename3.style.backgroundColor = "#ffffcc";
                   tename4.style.backgroundColor = "#ffffcc";
                    var tename5= document.getElementById('<%=txt_vehtype.ClientID %>');
                   var tename6= document.getElementById('<%=txt_visit1.ClientID %>');
                   tename5.style.backgroundColor = "#ffffcc";
                   tename6.style.backgroundColor = "#ffffcc";
                    var tename7= document.getElementById('<%=txt_vehno1.ClientID %>');
                   var tename8= document.getElementById('<%=txt_cty.ClientID %>');
                   tename7.style.backgroundColor = "#ffffcc";
                   tename8.style.backgroundColor = "#ffffcc";
                    var tename9= document.getElementById('<%=txt_stat.ClientID %>');
                   var tename10= document.getElementById('<%=txt_dis.ClientID %>');
                   tename9.style.backgroundColor = "#ffffcc";
                   tename10.style.backgroundColor = "#ffffcc";
                    var tename11= document.getElementById('<%=txt_str.ClientID %>');
                   tename11.style.backgroundColor = "#ffffcc";
                    document.getElementById('<%=TextBox1.ClientID %>').disabled = false;
            }
                companyclear();
            }
        }
        function withapclear() {
            document.getElementById('<%=sname.ClientID %>').value = "";
            document.getElementById('<%=txt_dpt1.ClientID %>').value = "";
            document.getElementById('<%=txt_desg1.ClientID %>').value = "";
            document.getElementById('<%=txt_type.ClientID %>').value = "";
            document.getElementById('<%=mblno.ClientID %>').value = "";
        }
        function withoutapclear() {
            document.getElementById('<%=txt_visitormeetstaffid.ClientID %>').value = "";
            document.getElementById('<%=txt_visitormeetstaffname.ClientID %>').value = "";
            document.getElementById('<%=txt_visitormeetstaffdept.ClientID %>').value = "";
            document.getElementById('<%=txt_visitormeetstaffdesg.ClientID %>').value = "";
            document.getElementById('<%=txt_visitormeetoffname.ClientID %>').value = "";
            document.getElementById('<%=txt_visitormeetoffdept.ClientID %>').value = "";
            document.getElementById('<%=txt_visitormeetoffdesg.ClientID %>').value = "";
            document.getElementById('<%=txt_visitormeetothername.ClientID %>').value = "";
            document.getElementById('<%=txt_visitormeetotherrel.ClientID %>').value = "";
            document.getElementById('<%=txt_visitormeetothermob.ClientID %>').value = "";
        }
        function visitorappoint() {
         var rb_withap = document.getElementById('<%=rb_withap.ClientID %>');
         var rb_withoutap=document.getElementById('<%=rb_withoutap.ClientID %>');
            if (rb_withap.checked) {
                var withap = document.getElementById('<%=div_withappoint.ClientID %>');
                withap.style.display = "block";
                var withoutap = document.getElementById('<%=div_withoutappoint.ClientID %>');
                withoutap.style.display = "none";
                var staff = document.getElementById('<%=div_visitormeetstaff.ClientID %>');
                staff.style.display = "none";
                var office = document.getElementById('<%=div_visitormeetoffice.ClientID %>');
                office.style.display = "none";
                var other = document.getElementById('<%=div_visitormeetother.ClientID %>');
                other.style.display = "none";
                withapclear();
            }
            else if (rb_withoutap.checked) {
                var withoutap = document.getElementById('<%=div_withoutappoint.ClientID %>');
                withoutap.style.display = "block";
                rbvisitormeet();
                var withap = document.getElementById('<%=div_withappoint.ClientID %>');
                withap.style.display = "none";
                
                withoutapclear();
            }
            visitorvehicle();
            return false;
        }
        function visitorvehicle() { 
          var rb_vehyes = document.getElementById('<%=rb_vehyes.ClientID %>');
          var rb_vehno = document.getElementById('<%=rb_vehno.ClientID %>');
            if (rb_vehyes.checked) {
                var vehyes = document.getElementById('<%=div_vehyes.ClientID %>');
                vehyes.style.display = "block";
            }
            else if (rb_vehno.checked) {
                var vehyes = document.getElementById('<%=div_vehyes.ClientID %>');
                vehyes.style.display = "none";
            }
            return false;
        }
         function visitorreturn() {
              var rb_ret = document.getElementById('<%=rb_ret.ClientID %>');
                var rb_notret = document.getElementById('<%=rb_notret.ClientID %>');
            if (rb_ret.checked) {
               document.getElementById('<%= ddl_hrs.ClientID %>').disabled = false;
               document.getElementById('<%= ddl_mins.ClientID %>').disabled = false;
               document.getElementById('<%= ddl_ampm.ClientID %>').disabled = false;
            }
            else if (rb_notret.checked) {
               document.getElementById('<%= ddl_hrs.ClientID %>').disabled = true;
               document.getElementById('<%= ddl_mins.ClientID %>').disabled = true;
               document.getElementById('<%= ddl_ampm.ClientID %>').disabled = true;
            }
            return false;
        }
         //magesh 4.6.18
        function visitormeetstaffdetbyid(txt1) {
            $.ajax({
                type: "POST",
                url: "GatePassEntryExit.aspx/getstaffdatabyid",
                data: '{Staff_Code: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    bindvisitormeetstaffdata(response.d);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function visitorstaffdet(txt) {
            $.ajax({
                type: "POST",
                url: "GatePassEntryExit.aspx/getvisitorstaffdet",
                data: '{Staff_Name: "' + txt + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    bind(response.d);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function bind(VisitorStaff) {
            var Staff_Name = VisitorStaff[0].Staff_Name;
            document.getElementById('<%=sname.ClientID %>').value = Staff_Name;
            var Department = VisitorStaff[0].Department;
            document.getElementById('<%=txt_dpt1.ClientID %>').value = Department;
            var Designation = VisitorStaff[0].Designation;
            document.getElementById('<%=txt_desg1.ClientID %>').value = Designation;
            var Staff_Type = VisitorStaff[0].Staff_Type;
            document.getElementById('<%=txt_type.ClientID %>').value = Staff_Type;
            var Mobile_No = VisitorStaff[0].Mobile_No;
            document.getElementById('<%=mblno.ClientID %>').value = Mobile_No;
            var img = VisitorStaff[0].Photo;
            document.getElementById('<%=image10.ClientID %>').src = img;
            document.getElementById('<%=image10.ClientID %>').value = img;
            return false;
        }
        function rbvisitormeet() {
        var rb_staff1=document.getElementById('<%=rb_staff1.ClientID %>');
        var rb_office1=document.getElementById('<%=rb_office1.ClientID %>');
        var rb_others1=document.getElementById('<%=rb_others1.ClientID %>');
            if (rb_staff1.checked) {
                var staff = document.getElementById('<%=div_visitormeetstaff.ClientID %>');
                staff.style.display = "block";
                var office = document.getElementById('<%=div_visitormeetoffice.ClientID %>');
                office.style.display = "none";
                var other = document.getElementById('<%=div_visitormeetother.ClientID %>');
                other.style.display = "none";
                withoutapclear();
            }
            else if (rb_office1.checked) {
                var staff = document.getElementById('<%=div_visitormeetstaff.ClientID %>');
                staff.style.display = "none";
                var office = document.getElementById('<%=div_visitormeetoffice.ClientID %>');
                office.style.display = "block";
                var other = document.getElementById('<%=div_visitormeetother.ClientID %>');
                other.style.display = "none";
                withoutapclear();
            }
            else if (rb_others1.checked) {
                var staff = document.getElementById('<%=div_visitormeetstaff.ClientID %>');
                staff.style.display = "none";
                var office = document.getElementById('<%=div_visitormeetoffice.ClientID %>');
                office.style.display = "none";
                var other = document.getElementById('<%=div_visitormeetother.ClientID %>');
                other.style.display = "block";
                withoutapclear();
            }
            return false;
        }
        function visitormeetname(txt1) {
            $.ajax({
                type: "POST",
                url: "GatePassEntryExit.aspx/getstaffdatabyid",
                data: '{Staff_Code: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    bindvisitormeetstaffdata(response.d);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function visitormeetstaffdetbyname(txt1) {
            $.ajax({
                type: "POST",
                url: "GatePassEntryExit.aspx/getstaffdatabyname",
                data: '{Staff_Name: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    bindvisitormeetstaffdata(response.d);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function visitormeetofficedetbyname(txt1) {
            $.ajax({
                type: "POST",
                url: "GatePassEntryExit.aspx/getstaffdatabyname",
                data: '{Staff_Name: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    bindvisitormeetofficedata(response.d);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function bindvisitormeetstaffdata(VisitorStaff) {
            var Staff_Code = VisitorStaff[0].Staff_Code;
            document.getElementById('<%=txt_visitormeetstaffid.ClientID %>').value = Staff_Code;
            var Staff_Name = VisitorStaff[0].Staff_Name;
            document.getElementById('<%=txt_visitormeetstaffname.ClientID %>').value = Staff_Name;
            var Department = VisitorStaff[0].Department;
            document.getElementById('<%=txt_visitormeetstaffdept.ClientID %>').value = Department;
            var Designation = VisitorStaff[0].Designation;
            document.getElementById('<%=txt_visitormeetstaffdesg.ClientID %>').value = Designation;
            return false;
        }
        function bindvisitormeetofficedata(VisitorStaff) {
            var Staff_Name = VisitorStaff[0].Staff_Name;
            document.getElementById('<%=txt_visitormeetoffname.ClientID %>').value = Staff_Name;
            var Department = VisitorStaff[0].Department;
            document.getElementById('<%=txt_visitormeetoffdept.ClientID %>').value = Department;
            var Designation = VisitorStaff[0].Designation;
            document.getElementById('<%=txt_visitormeetoffdesg.ClientID %>').value = Designation;
            return false;
        }
        function checkcname(txt1) {
            $.ajax({
                type: "POST",
                url: "GatePassEntryExit.aspx/CheckCompanyName",
                data: '{company_name: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccessCname,
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function OnSuccessCname(response) {
            var mesg = $("#cnamemsg")[0];
            switch (response.d) {
                case "0":
                    // bindData();
                    mesg.style.color = "red";
                    mesg.innerHTML = "Company name not exist";
                    compdetclear();
                    break;
                case "1":
                    visitorcompdet();
                    mesg.style.color = "green";
                    mesg.innerHTML = "Available";
                    break;
                case "error":
                    mesg.style.color = "red";
                    mesg.innerHTML = "Error occurred";
                    break;
            }
        }
//        function visitorgatepass(txt1) {
//            $.ajax({
//                type: "POST",
//                url: "GatePassEntryExit.aspx/getvisitorgate1",
//                data: '{Gatepassno: "' + txt1 + '"}',
//                contentType: "application/json; charset=utf-8",
//                dataType: "json",
//                success: function (response) {
//                    bindvisitorgatepass(response.d);
//                },
//                failure: function (response) {
//                    alert(response);
//                }
//            });
//        }
         function visitorcompdet(txt1) {
        
            $.ajax({
                type: "POST",
                url: "GatePassEntryExit.aspx/getvisitorcompdata",
                data: '{Company_Name: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    bindvisitorcompdet(response.d);
                    
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function visitorcompdetmobileno(txt1) {
            $.ajax({
                type: "POST",
                url: "GatePassEntryExit.aspx/getvisitorcompdatamobileno",
                data: '{mobileNo: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    bindvisitorcompdet(response.d);
                    
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }

//        function bindvisitorgatepass(gate)
//        {
//          var Company_Name = gate[0].Company_Name;
//            document.getElementById('<%=txt_compname.ClientID %>').value = Company_Name;
//            var  Company_Nameper = gate[0].Company_Nameperson;
//            document.getElementById('<%=txt_name4.ClientID %>').value = Company_Nameper;
////            var Company_design = gate[0].Company_designation;
////            document.getElementById('<%=txt_desgn.ClientID %>').value = Company_design;
//            var Company_dept = gate[0].Company_department;
//            document.getElementById('<%=txt_mno.ClientID %>').value = Company_dept;
//        } 
function vis(VisitorCompany) { 

  var mage= document.getElementById("<%=Hidden1.ClientID %>").value;
  
  if(mage=="0" || mage=="" )
  {
   var visitor = document.getElementById('<%=div_withoutappoint.ClientID %>');
            visitor.style.display = "block";
                var visitorstaff = document.getElementById('<%=div_visitormeetstaff.ClientID %>');
            visitorstaff.style.display = "block";
                     document.getElementById('<%=rb_withoutap.ClientID %>').checked=true;
                    
        var rb_staff1=document.getElementById('<%=rb_staff1.ClientID %>');
        var rb_office1=document.getElementById('<%=rb_office1.ClientID %>');
        var rb_others1=document.getElementById('<%=rb_others1.ClientID %>');
            if (rb_staff1.checked) {
                var staff = document.getElementById('<%=div_visitormeetstaff.ClientID %>');
                staff.style.display = "block";
                var office = document.getElementById('<%=div_visitormeetoffice.ClientID %>');
                office.style.display = "none";
                var other = document.getElementById('<%=div_visitormeetother.ClientID %>');
                other.style.display = "none";
            }
            else if (rb_office1.checked) {
                var staff = document.getElementById('<%=div_visitormeetstaff.ClientID %>');
                staff.style.display = "none";
                var office = document.getElementById('<%=div_visitormeetoffice.ClientID %>');
                office.style.display = "block";
                var other = document.getElementById('<%=div_visitormeetother.ClientID %>');
                other.style.display = "none";
            }
            else if (rb_others1.checked) {
                var staff = document.getElementById('<%=div_visitormeetstaff.ClientID %>');
                staff.style.display = "none";
                var office = document.getElementById('<%=div_visitormeetoffice.ClientID %>');
                office.style.display = "none";
                var other = document.getElementById('<%=div_visitormeetother.ClientID %>');
                other.style.display = "block";
            }
  }
  else
  {
  
             document.getElementById('<%=rb_withap.ClientID %>').checked=true;
             var visitor = document.getElementById('<%=div_withappoint.ClientID %>');
            visitor.style.display = "block";
  }

}
        function bindvisitorcompdet(VisitorCompany) { 

            var appnmt = VisitorCompany[0].Appointment;
             var msg = VisitorCompany[0].statusmsgvis1;
             if(appnmt=="1")
             {      
             var Company_Name = VisitorCompany[0].Company_Name;
            
            document.getElementById('<%=txt_compname.ClientID %>').value = Company_Name;
            var  Company_Nameper = VisitorCompany[0].Company_Nameperson;
            document.getElementById('<%=txt_name4.ClientID %>').value = Company_Nameper;
            var Company_design = VisitorCompany[0].Company_designation;
            document.getElementById('<%=txt_desgn.ClientID %>').value = Company_design;
            var Company_dept = VisitorCompany[0].Company_department;
            document.getElementById('<%=txt_dep.ClientID %>').value = Company_dept;
            var Company_street = VisitorCompany[0].Company_street;
            document.getElementById('<%=txt_str.ClientID %>').value = Company_street;
           
            var Company_City = VisitorCompany[0].Company_City;
            document.getElementById('<%=txt_cty.ClientID %>').value = Company_City;
             
            var Company_District = VisitorCompany[0].Company_District;
            document.getElementById('<%=txt_dis.ClientID %>').value = Company_District;
         
            var Company_State = VisitorCompany[0].Company_State;
            document.getElementById('<%=txt_stat.ClientID %>').value = Company_State;
                 
            var phone_no = VisitorCompany[0].phone_no;
     
            var mobile_no = VisitorCompany[0].mobile_no;
            document.getElementById('<%=txt_mno.ClientID %>').value = mobile_no; 


             document.getElementById('<%=rb_withap.ClientID %>').checked=true;
             var visitor = document.getElementById('<%=div_withappoint.ClientID %>');
            visitor.style.display = "block";
             var staffname = VisitorCompany[0].staffname;
            document.getElementById('<%=sname.ClientID %>').value = staffname;
              var staffdept = VisitorCompany[0].staff_dept;
            document.getElementById('<%=txt_dpt1.ClientID %>').value = staffdept;
              var stafftype = VisitorCompany[0].staff_type;
            document.getElementById('<%=txt_type.ClientID %>').value = stafftype;
              var staffmob = VisitorCompany[0].staff_mob;
            document.getElementById('<%=mblno.ClientID %>').value = staffmob;
               var staffdes= VisitorCompany[0].staff_design;
            document.getElementById('<%=txt_desg1.ClientID %>').value = staffdes;
             var purposeofvisit= VisitorCompany[0].purposeofvisit;
            document.getElementById('<%=txt_visit1.ClientID %>').value = purposeofvisit;
              var visitorss = document.getElementById('<%=div_withoutappoint.ClientID %>');
            visitorss.style.display = "none";
             var staff = document.getElementById('<%=div_visitormeetstaff.ClientID %>');
                staff.style.display = "none";
                var office = document.getElementById('<%=div_visitormeetoffice.ClientID %>');
                office.style.display = "none";
                var other = document.getElementById('<%=div_visitormeetother.ClientID %>');
                other.style.display = "none";

             }
             else
             {

//             var mobi=document.getElementById('<%=txt_mno.ClientID %>').value;
//              if(mobi.length != 10)
//              {

//              statusmsgvis.innerHTML = "Please Enter Valid 10 - Digits Mobile Numbers";
//               statusmsgvis.style.color = "red";
//              }
//               if(mobi.length == "")
//              {
//               statusmsgvis.innerHTML = "";
//              
//              }
 
             var Company_Name = VisitorCompany[0].Company_Name;
          
            document.getElementById('<%=txt_compname.ClientID %>').value = Company_Name;
             document.getElementById('<%=rb_withoutap.ClientID %>').checked=true;
                var visitor = document.getElementById('<%=div_withoutappoint.ClientID %>');
            visitor.style.display = "block";
             var rb_staff1=document.getElementById('<%=rb_staff1.ClientID %>');
        var rb_office1=document.getElementById('<%=rb_office1.ClientID %>');
        var rb_others1=document.getElementById('<%=rb_others1.ClientID %>');
           
             if (rb_office1.checked) {
                var staff = document.getElementById('<%=div_visitormeetstaff.ClientID %>');
                staff.style.display = "none";
                var office = document.getElementById('<%=div_visitormeetoffice.ClientID %>');
                office.style.display = "block";
                var other = document.getElementById('<%=div_visitormeetother.ClientID %>');
                other.style.display = "none";
            }
            else if (rb_others1.checked) {
                var staff = document.getElementById('<%=div_visitormeetstaff.ClientID %>');
                staff.style.display = "none";
                var office = document.getElementById('<%=div_visitormeetoffice.ClientID %>');
                office.style.display = "none";
                var other = document.getElementById('<%=div_visitormeetother.ClientID %>');
                other.style.display = "block";
            }
             else if (rb_staff1.checked) {
                var staff = document.getElementById('<%=div_visitormeetstaff.ClientID %>');
                staff.style.display = "block";
                var office = document.getElementById('<%=div_visitormeetoffice.ClientID %>');
                office.style.display = "none";
                var other = document.getElementById('<%=div_visitormeetother.ClientID %>');
                other.style.display = "none";
            }
//                var visitorstaff = document.getElementById('<%=div_visitormeetstaff.ClientID %>');
//            visitorstaff.style.display = "block";
             } 
              var mobi=document.getElementById('<%=txt_mno.ClientID %>').value;
              if(mobi.length != 10)
              {
              statusmsgvis.innerHTML = "Please Enter Valid 10 - Digits Mobile Numbers";
               statusmsgvis.style.color = "red";
              }
                                
               if(mobi.length == "")
              {
               statusmsgvis.innerHTML = "";
              
              } 
             if(msg=="0")
              {
              statusmsgvis.innerHTML = "Please Check Your Request";
               statusmsgvis.style.color = "red";
              }           
            if(document.getElementById('<%=rb_visitout.ClientID %>').checked)
            {
            var vehical=VisitorCompany[0].vehical.toString();
            if(vehical=="1")
            {
                document.getElementById('<%=rb_vehno.ClientID %>').checked=true;
                document.getElementById('<%=rb_vehyes.ClientID %>').checked=false;
                var visitor = document.getElementById('<%=div_vehyes.ClientID %>');
                visitor.style.display = "block";
            }
            else
            {
                document.getElementById('<%=rb_vehno.ClientID %>').checked=false;
                document.getElementById('<%=rb_vehyes.ClientID %>').checked=true;
                var VehType = VisitorCompany[0].VehType;
                document.getElementById('<%=txt_vehtype.ClientID %>').value = VehType;
                var VehRegNo = VisitorCompany[0].VehRegNo;
                document.getElementById('<%=txt_vehno1.ClientID %>').value = VehRegNo;
            
                var tomeet = VisitorCompany[0].tomeet;
                document.getElementById('<%=rb_others1.ClientID %>').checked=false;
                document.getElementById('<%=rb_office1.ClientID %>').checked=false;
                document.getElementById('<%=rb_staff1.ClientID %>').checked=false;
            if(tomeet=="0")
            { 
                var Staff_Code = VisitorCompany[0].Staff_Code;
                document.getElementById('<%=rb_staff1.ClientID %>').checked=true; 
                document.getElementById('<%=txt_visitormeetstaffid.ClientID %>').value = Staff_Code;
                visitormeetstaffdetbyid(Staff_Code);
            }
            else if(tomeet=="1")
            {
                document.getElementById('<%=rb_office1.ClientID %>').checked=true;
                var Staff_name = VisitorCompany[0].staffname;
                document.getElementById('<%=txt_visitormeetoffname.ClientID %>').value = Staff_name;
                visitormeetofficedetbyname(Staff_name);
            }
            else if(tomeet=="2")
            {
                document.getElementById('<%=rb_others1.ClientID %>').checked=true;
                var othername= VisitorCompany[0].othername;
                document.getElementById('<%=txt_visitormeetothername.ClientID %>').value = othername;
                var Relationship= VisitorCompany[0].Relationship;
                document.getElementById('<%=txt_visitormeetotherrel.ClientID %>').value = Relationship;
                var MobileNo= VisitorCompany[0].MobileNo;
                document.getElementById('<%=txt_visitormeetothermob.ClientID %>').value = MobileNo;
            }
            rbvisitormeetwithoutclear();
                var returnvisitor = VisitorCompany[0].returnvisitor;
                document.getElementById('<%=rb_ret.ClientID %>').checked=false;
                document.getElementById('<%=rb_notret.ClientID %>').checked=false;
                if(returnvisitor=="0")
                {
                document.getElementById('<%=rb_ret.ClientID %>').checked=true;
                }
                else
                {
                document.getElementById('<%=rb_notret.ClientID %>').checked=true;
                }

                 //magesh 7.6.18
               // visitorreturn();
                          
             }  
             }
               
            return false;
        }
        function rbvisitormeetwithoutclear() {
        var rb_staff1=document.getElementById('<%=rb_staff1.ClientID %>');
        var rb_office1=document.getElementById('<%=rb_office1.ClientID %>');
        var rb_others1=document.getElementById('<%=rb_others1.ClientID %>');
            if (rb_staff1.checked) {
                var staff = document.getElementById('<%=div_visitormeetstaff.ClientID %>');
                staff.style.display = "block";
                var office = document.getElementById('<%=div_visitormeetoffice.ClientID %>');
                office.style.display = "none";
                var other = document.getElementById('<%=div_visitormeetother.ClientID %>');
                other.style.display = "none";
            }
            else if (rb_office1.checked) {
                var staff = document.getElementById('<%=div_visitormeetstaff.ClientID %>');
                staff.style.display = "none";
                var office = document.getElementById('<%=div_visitormeetoffice.ClientID %>');
                office.style.display = "block";
                var other = document.getElementById('<%=div_visitormeetother.ClientID %>');
                other.style.display = "none";
            }
            else if (rb_others1.checked) {
                var staff = document.getElementById('<%=div_visitormeetstaff.ClientID %>');
                staff.style.display = "none";
                var office = document.getElementById('<%=div_visitormeetoffice.ClientID %>');
                office.style.display = "none";
                var other = document.getElementById('<%=div_visitormeetother.ClientID %>');
                other.style.display = "block";
            }
            return false;
        }
        //---------------------------------for material----------------------------------------
        function materialbtn() {
            var visitor = document.getElementById('<%=div_visitor.ClientID %>');
            visitor.style.display = "none";
            var newid = document.getElementById('<%=div_student.ClientID %>');
            newid.style.display = "none";
            var staff = document.getElementById('<%=div_staff.ClientID %>');
            staff.style.display = "none";
            var parents = document.getElementById('<%=div_parent.ClientID %>');
            parents.style.display = "none";
            var material = document.getElementById('<%=div_material.ClientID %>');
            material.style.display = "block";
            //document.getElementById("div_material").style.overflow = "scroll";
            var vehicle = document.getElementById('<%=div_vehicle.ClientID %>');
            vehicle.style.display = "none";
            rbmaterial();
            materialentryby();
            document.getElementById('<%=studenttd.ClientID %>').style.backgroundColor = "#FFFFFF";
            document.getElementById('<%=stafftd.ClientID %>').style.backgroundColor =  "#FFFFFF";
            document.getElementById('<%=parenttd.ClientID %>').style.backgroundColor =  "#FFFFFF";
            document.getElementById('<%=visitortd.ClientID %>').style.backgroundColor =  "#FFFFFF";
            document.getElementById('<%=materialtd.ClientID %>').style.backgroundColor = "#c4c4c4";
            document.getElementById('<%=vehicletd.ClientID %>').style.backgroundColor =  "#FFFFFF";
            return false;
        }
        function rbmaterial() {
          var rb_ordmaterial = document.getElementById('<%=rb_ordmaterial.ClientID %>');
          var rb_other = document.getElementById('<%=rb_other.ClientID %>');
          var rb_service = document.getElementById('<%=rb_service.ClientID %>');
            if (rb_ordmaterial.checked) {
                var order = document.getElementById('<%=div_ordermaterial.ClientID %>');
                order.style.display = "block";
                var other = document.getElementById('<%=div_material_others.ClientID %>');
                other.style.display = "none";
                materialclear();
            }
            else if (rb_other.checked) {
                var other = document.getElementById('<%=div_material_others.ClientID %>');
                other.style.display = "block";
                var order = document.getElementById('<%=div_ordermaterial.ClientID %>');
                order.style.display = "none";
                materialclear();
            }
            else if (rb_service.checked) {
                var order = document.getElementById('<%=div_ordermaterial.ClientID %>');
                order.style.display = "none";
                var other = document.getElementById('<%=div_material_others.ClientID %>');
                other.style.display = "none";
                materialclear();
            }
            materialentryby();
            return false;
        }
        function materialentryclear() {
            document.getElementById('<%=txt_vehno.ClientID %>').value = "";
            document.getElementById('<%=txt_vehitype.ClientID %>').value = "";
            document.getElementById('<%=txt_drivname.ClientID %>').value = "";
            document.getElementById('<%=txt_mobno.ClientID %>').value = "";
            document.getElementById('<%=txt_vehino.ClientID %>').value = "";
            document.getElementById('<%=txt_vehitype1.ClientID %>').value = "";
            document.getElementById('<%=txt_bbyname.ClientID %>').value = "";
            document.getElementById('<%=txt_mobno1.ClientID %>').value = "";
        }
        function materialentryby() {
          var rb_materialinsveh = document.getElementById('<%=rb_materialinsveh.ClientID %>');
           var rb_materialotherveh = document.getElementById('<%=rb_materialotherveh.ClientID %>');
            if (rb_materialinsveh.checked == true) {
                var matinsveh = document.getElementById('<%=div_metr_entryby.ClientID %>');
                matinsveh.style.display = "block";
                var matothveh = document.getElementById('<%=div_metr_others.ClientID %>');
                matothveh.style.display = "none";
                materialentryclear();
            }
            else if (rb_materialotherveh.checked == true) {
                var matothveh = document.getElementById('<%=div_metr_others.ClientID %>');
                matothveh.style.display = "block";
                var matinsveh = document.getElementById('<%=div_metr_entryby.ClientID %>');
                matinsveh.style.display = "none";
                materialentryclear();
            }
        }
        function checkpono(txt1) {
            $.ajax({
                type: "POST",
                url: "GatePassEntryExit.aspx/CheckPONo",
                data: '{order_code: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccessPono,
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function OnSuccessPono(response) {
            var mesg = $("#ponomsg")[0];
            switch (response.d) {
                case "0":
                    // bindData();
                    mesg.style.color = "red";
                    mesg.innerHTML = "Order No. not exist";
                    ponoclear();
                    break;
                case "1":
                    getmatpurdet();
                    mesg.style.color = "green";
                    mesg.innerHTML = "Available";
                    break;
                case "error":
                    mesg.style.color = "red";
                    mesg.innerHTML = "Error occurred";
                    break;
            }
        }
        function getmatpurdet(txt1) {
            $.ajax({
                type: "POST",
                url: "GatePassEntryExit.aspx/getmaterialpurchasedata",
                data: '{Order_Code: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    bindmatpurdata(response.d);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function bindmatpurdata(MaterialPurchase) {
            var item = document.getElementById('<%=div_matitem.ClientID %>');
            item.style.display = "block";
            var root = document.getElementById('mydiv');
            try {
                var tblId = document.getElementById('tblGridValue');
                if (tblId != null) {
                    root.removeChild(tblId);
                }
            }
            catch (e) {
            }
            var Vendor_Name = MaterialPurchase[0].Vendor_Name;
            document.getElementById('<%=txt_suppliername.ClientID %>').value = Vendor_Name;
            var vendor_address = MaterialPurchase[0].vendor_address;
            document.getElementById('<%=txt_addr.ClientID %>').value = vendor_address;
            var Vendor_City = MaterialPurchase[0].Vendor_City;
            document.getElementById('<%=txt_city1.ClientID %>').value = Vendor_City;
            var Vendor_District = MaterialPurchase[0].Vendor_District;
            document.getElementById('<%=txt_dist.ClientID %>').value = Vendor_District;
            var Vendor_State = MaterialPurchase[0].Vendor_State;
            document.getElementById('<%=txt_state1.ClientID %>').value = Vendor_State;
            var pin = MaterialPurchase[0].pin;
            document.getElementById('<%=txt_pincode1.ClientID %>').value = pin;
            var Contact_Desig = MaterialPurchase[0].Contact_Desig;
            document.getElementById('<%=txt_contperson.ClientID %>').value = Contact_Desig;
            var Contact_Name = MaterialPurchase[0].Contact_Name;
            document.getElementById('<%=txt_name3.ClientID %>').value = Contact_Name;
            var ContactMobileNo = MaterialPurchase[0].ContactMobileNo;
            document.getElementById('<%=txt_mobileno.ClientID %>').value = ContactMobileNo;
            var tab = document.createElement('table');
            tab.setAttribute("id", "tblGridValue");
            tab.setAttribute("class", "tableStyle");
            tab.setAttribute("cellspacing", "3px");
            tab.setAttribute("rules","all");
            var tbo = document.createElement('tbody');
            var row, cell;
            $.each(MaterialPurchase, function (index, MaterialPurchase) {
                row = document.createElement('tr');
                row.setAttribute("class", "tableRaw");
                for (var j = 0; j < 5; j++) {
                    cell = document.createElement('td');
                    cell.setAttribute("width", "30px");
                    
                    var sno = index + 1;
                 
                var x = document.createElement("INPUT");
                x.setAttribute("type", "checkbox"); 
                    var item_code = MaterialPurchase.item_code;
                    var item_name = MaterialPurchase.item_name;
                    var app_qty = MaterialPurchase.app_qty;
                    if (j == 0) {
                        var hiddenId = document.createElement("input");
                        hiddenId.setAttribute("type", "hidden");
                        hiddenId.setAttribute("id", "hfRow_" + item_code);
                        hiddenId.setAttribute("value", item_code);
                        cell.appendChild(hiddenId);
                        cell.appendChild(document.createTextNode(sno));
                    }
                     if (j == 1) {
                        var hiddenId = document.createElement("INPUT");
                        hiddenId.setAttribute("type", "checkbox");
                        hiddenId.setAttribute("id", "hfRow_" + item_code);
                        hiddenId.setAttribute("value", item_code);
                        cell.appendChild(hiddenId);
                        
                    }
                    else if (j == 2) {
                        var spanValue = document.createElement("span");
                        cell.setAttribute("width", "100px");
                        cell.setAttribute("align", "center");
                        spanValue.setAttribute("display", "inline-block");
                        spanValue.appendChild(document.createTextNode(item_code));
                        cell.appendChild(spanValue);
                    }
                    else if (j == 3) {
                        cell.setAttribute("width", "120px");
                        cell.setAttribute("align", "center");
                        cell.appendChild(document.createTextNode(item_name));
                    }
                    else if (j == 4) {
                        cell.setAttribute("width", "120px");
                        cell.setAttribute("align", "center");
                        cell.appendChild(document.createTextNode(app_qty));
                    }
                    row.appendChild(cell);
                }
                tbo.appendChild(row);
            });
            tab.appendChild(tbo);
            root.appendChild(tab);
            return false;
        }
        function getmatentbyinsvehdet(txt1) {
            $.ajax({
                type: "POST",
                url: "GatePassEntryExit.aspx/getdriverdata",
                data: '{Veh_ID: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    bindmaterialinsvehdata(response.d);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function bindmaterialinsvehdata(Driver) {
            var name = Driver[0].Name;
            document.getElementById('<%=txt_drivname.ClientID %>').value = name;
            var Veh_ID = Driver[0].Veh_ID;
            document.getElementById('<%=txt_vehno.ClientID %>').value = Veh_ID;
            var Mobile_No = Driver[0].Mobile_No;
            document.getElementById('<%=txt_mobno.ClientID %>').value = Mobile_No;
            var Veh_Type = Driver[0].Veh_Type;
            document.getElementById('<%=txt_vehitype.ClientID %>').value = Veh_Type;
        }
        function getitemdet(txt1) {
            $.ajax({
                type: "POST",
                url: "GatePassEntryExit.aspx/getitemdata",
                data: '{item_name: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    binditemdata(response.d);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function binditemdata(ItemDet) {
            var item = document.getElementById('<%=div_matitem.ClientID %>');
            item.style.display = "block";
            var item_name = ItemDet[0].item_name;
            document.getElementById('<%=txt_itemname.ClientID %>').value = item_name;
            var item_unit = ItemDet[0].item_unit;
            document.getElementById('<%=txt_measure.ClientID %>').value = item_unit;
        }
        //----------------------------------------for vehicle-------------------------------------
        function vehiclebtn() {
            var visitor = document.getElementById('<%=div_visitor.ClientID %>');
            visitor.style.display = "none";
            var newid = document.getElementById('<%=div_student.ClientID %>');
            newid.style.display = "none";
            var staff = document.getElementById('<%=div_staff.ClientID %>');
            staff.style.display = "none";
            var parents = document.getElementById('<%=div_parent.ClientID %>');
            parents.style.display = "none";
            var material = document.getElementById('<%=div_material.ClientID %>');
            material.style.display = "none";
            var vehicle = document.getElementById('<%=div_vehicle.ClientID %>');
            vehicle.style.display = "block";
         //   document.getElementById("div_vehicle").style.overflow = "scroll";
            rbvehicletype();
            vehicelapstatus();
            document.getElementById('<%=studenttd.ClientID %>').style.backgroundColor = "#FFFFFF";
            document.getElementById('<%=stafftd.ClientID %>').style.backgroundColor =  "#FFFFFF";
            document.getElementById('<%=parenttd.ClientID %>').style.backgroundColor =  "#FFFFFF";
            document.getElementById('<%=visitortd.ClientID %>').style.backgroundColor =  "#FFFFFF";
            document.getElementById('<%=materialtd.ClientID %>').style.backgroundColor = "#FFFFFF";
            document.getElementById('<%=vehicletd.ClientID %>').style.backgroundColor =  "#c4c4c4";
            return false;
        }
        function rbvehicletype() {
          var rb_instuveh = document.getElementById('<%=rb_instuveh.ClientID %>');
            var rb_otherveh = document.getElementById('<%=rb_otherveh.ClientID %>');
            if (rb_instuveh.checked == true) {
                var inst = document.getElementById('<%=div_instvehicle.ClientID %>');
                inst.style.display = "block";
                var other = document.getElementById('<%=div_othervehicle.ClientID %>');
                other.style.display = "none";
                vehicleclear();
            }
            else if (rb_otherveh.checked == true) {
                var inst = document.getElementById('<%=div_instvehicle.ClientID %>');
                inst.style.display = "none";
                var other = document.getElementById('<%=div_othervehicle.ClientID %>');
                other.style.display = "block";
                vehicleclear();
            }
            vehicelapstatus();
        }
        function checkvehid(txt1) {
            $.ajax({
                type: "POST",
                url: "GatePassEntryExit.aspx/CheckVehicleID",
                data: '{Veh_ID: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccessVeh,
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function OnSuccessVeh(response) {
            var mesg = $("#vehmsg")[0];
            switch (response.d) {
                case "0":
                    // bindData();
                    mesg.style.color = "red";
                    mesg.innerHTML = "Vehicle No not exist";
                    vehidclear();
                    break;
                case "1":
                    getvehicledetail();
                    mesg.style.color = "green";
                    mesg.innerHTML = "Available";
                    break;
                case "error":
                    mesg.style.color = "red";
                    mesg.innerHTML = "Error occurred";
                    break;
            }
        }
        function getvehicledetail(txt1) {
            $.ajax({
                type: "POST",
                url: "GatePassEntryExit.aspx/getdriverdata",
                data: '{Veh_ID: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    bindvehicleinstdata(response.d);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function bindvehicleinstdata(Driver) {
            var name = Driver[0].Name;
            document.getElementById('<%=txt_driver1.ClientID %>').value = name;
            var Veh_ID = Driver[0].Veh_ID;
            document.getElementById('<%=txt_vehicleno2.ClientID %>').value = Veh_ID;
            var Route = Driver[0].Route;
            document.getElementById('<%=txt_rut.ClientID %>').value = Route;
            var Insurance_Date = Driver[0].Insurance_Date;
            document.getElementById('<%=txt_insurstatus.ClientID %>').value = Insurance_Date;
            var FC_Date = Driver[0].FC_Date;
            document.getElementById('<%=txt_fcstatus.ClientID %>').value = FC_Date;
            var Renew_Date = Driver[0].Renew_Date;
            document.getElementById('<%=txt_licstatus.ClientID %>').value = Renew_Date;
        }
        function vehicelapstatus() {
         var rb_appstyes = document.getElementById('<%=rb_appstyes.ClientID %>');
          var rb_appstno = document.getElementById('<%=rb_appstno.ClientID %>');
            if (rb_appstyes.checked == true) {
                var appyes = document.getElementById('<%=appstatus_yes.ClientID %>');
                appyes.style.display = "block";
            }
            else if (rb_appstno.checked == true) {
                var appyes = document.getElementById('<%=appstatus_yes.ClientID %>');
                appyes.style.display = "none";
            }
            return false;
        }
        function getvehiappdet(txt1) {
            $.ajax({
                type: "POST",
                url: "GatePassEntryExit.aspx/getstaffdatabyname",
                data: '{Staff_Name: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    bindvehiappdata(response.d);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function bindvehiappdata(VisitorStaff) {
            var Staff_Name = VisitorStaff[0].Staff_Name;
            document.getElementById('<%=txt_personname.ClientID %>').value = Staff_Name;
            var Department = VisitorStaff[0].Department;
            document.getElementById('<%=txt_depart.ClientID %>').value = Department;
            var Designation = VisitorStaff[0].Designation;
            document.getElementById('<%=txt_design.ClientID %>').value = Designation;
            return false;
        }
        //--------------------------------clear methods--------------------------------------
        function studrnoclear() {
            document.getElementById('<%=txt_name.ClientID %>').value = "";
            document.getElementById('<%=txt_studtype.ClientID %>').value = "";
            document.getElementById('<%=txt_degree.ClientID %>').value = "";
            document.getElementById('<%=txt_dept.ClientID %>').value = "";
            document.getElementById('<%=txt_sem.ClientID %>').value = "";
            document.getElementById('<%=txt_sec.ClientID %>').value = "";
           document.getElementById('<%=txt_apstaff.ClientID %>').value="";
           document.getElementById('<%=txt_apdept.ClientID %>').value="";
           document.getElementById('<%=txt_apdesgn.ClientID %>').value="";
            document.getElementById('<%=image2.ClientID %>').src = "";
            document.getElementById('<%=image2.ClientID %>').value = "";
            document.getElementById('<%=image4.ClientID %>').src = "";
            document.getElementById('<%=image4.ClientID %>').value = "";
           // rnomsg.innerHTML = "";
            statusmsg.innerHTML = "";
        }
        function smartclear() {
        document.getElementById('<%=txt_rollno.ClientID %>').value = "";
            document.getElementById('<%=txt_name.ClientID %>').value = "";
            document.getElementById('<%=txt_studtype.ClientID %>').value = "";
            document.getElementById('<%=txt_degree.ClientID %>').value = "";
            document.getElementById('<%=txt_dept.ClientID %>').value = "";
            document.getElementById('<%=txt_sem.ClientID %>').value = "";
            document.getElementById('<%=txt_sec.ClientID %>').value = "";
           document.getElementById('<%=txt_apstaff.ClientID %>').value="";
           document.getElementById('<%=txt_apdept.ClientID %>').value="";
           document.getElementById('<%=txt_apdesgn.ClientID %>').value="";
            document.getElementById('<%=image2.ClientID %>').src = "";
            document.getElementById('<%=image2.ClientID %>').value = "";
            document.getElementById('<%=image4.ClientID %>').src = "";
            document.getElementById('<%=image4.ClientID %>').value = "";
           // rnomsg.innerHTML = "";
            statusmsg.innerHTML = "";
        }
        function studentclear() {
            var stud = document.getElementById('<%=div_student.ClientID %>');
            stud.style.display = "block";
            document.getElementById('<%=txt_rollno.ClientID %>').value = "";
            document.getElementById('<%=txt_smart.ClientID %>').value = "";
            document.getElementById('<%=txt_name.ClientID %>').value = "";
            document.getElementById('<%=txt_studtype.ClientID %>').value = "";
            document.getElementById('<%=txt_degree.ClientID %>').value = "";
            document.getElementById('<%=txt_dept.ClientID %>').value = "";
            document.getElementById('<%=txt_sem.ClientID %>').value = "";
            document.getElementById('<%=txt_sec.ClientID %>').value = "";
            document.getElementById('<%=image2.ClientID %>').src = "";
            document.getElementById('<%=image2.ClientID %>').value = "";
            document.getElementById('<%=image4.ClientID %>').src = "";
            document.getElementById('<%=image4.ClientID %>').value = "";
            document.getElementById('<%=txt_apstaff.ClientID %>').value = "";
            document.getElementById('<%=txt_apdesgn.ClientID %>').value = "";
            document.getElementById('<%=txt_apdept.ClientID %>').value = "";
            document.getElementById('<%=txt_purpose1.ClientID %>').value = "";
            document.getElementById('<%=txt_ownvehno.ClientID %>').value = "";
            document.getElementById('<%=txt_owndrivername.ClientID %>').value = "";
            document.getElementById('<%=txt_ownmob.ClientID %>').value = "";
            document.getElementById('<%=txt_vehicleno.ClientID %>').value = "";
            document.getElementById('<%=txt_route.ClientID %>').value = "";
            document.getElementById('<%=txt_drop.ClientID %>').value = "";
            document.getElementById('<%=txt_drivername.ClientID %>').value = "";
            document.getElementById('<%=txt_mobile.ClientID %>').value = "";
            
               
                document.getElementById('<%=imageregvisitfar.ClientID %>').src = "";
                document.getElementById('<%=imageregvisitfar.ClientID %>').value = "";
             document.getElementById('<%=imageregvisitmon.ClientID %>').src = "";
            document.getElementById('<%=imageregvisitmon.ClientID %>').value = "";
             document.getElementById('<%=imageregvisitgaur.ClientID %>').src = "";
            document.getElementById('<%=imageregvisitgaur.ClientID %>').value = "";

            rnomsg.innerHTML = "";
            smarterr.innerHTML="";
            statusmsg.innerHTML = "";
            var date = document.getElementById('<%=txt_date.ClientID %>').value;
            document.getElementById('<%=txt_apdate.ClientID %>').value = date;
            var time = document.getElementById('<%=txt_time.ClientID %>').value;
            document.getElementById('<%=txt_aptime.ClientID %>').value = time;
          //  rb_in.checked = true;
            //  rb_apprno.checked = true;
           // rb_vehother.checked = true;
        }
        function staffcodeclear() {
            document.getElementById('<%=txt_staffname.ClientID %>').value = "";
            document.getElementById('<%=txt_staffdept.ClientID %>').value = "";
            document.getElementById('<%=txt_desg.ClientID %>').value = "";
            document.getElementById('<%=txt_staff_type.ClientID %>').value = "";
            document.getElementById('<%=imagestaff.ClientID %>').src = "";
            document.getElementById('<%=imagestaff.ClientID %>').value = "";
        }
        function staffvehicleclear()
        {
            document.getElementById('<%=txt_staff_ownvehilno.ClientID %>').value = "";
            document.getElementById('<%=txt_staff_owndrivername.ClientID %>').value = "";
            document.getElementById('<%=txt_staff_ownmobno.ClientID %>').value = "";
            document.getElementById('<%=txt_staff_vehilno.ClientID %>').value = "";
            document.getElementById('<%=txt_staff_route.ClientID %>').value = "";
            document.getElementById('<%=txt_staff_drop.ClientID %>').value = "";
            document.getElementById('<%=txt_staff_drvname.ClientID %>').value = "";
            document.getElementById('<%=txt_staff_mob.ClientID %>').value = "";
        }
        function staffclear() {
            var staff = document.getElementById('<%=div_staff.ClientID %>');
            staff.style.display = "block";
             document.getElementById('<%=txt_staff_smartcard.ClientID %>').value = "";
            document.getElementById('<%=txt_staffid.ClientID %>').value = "";
            document.getElementById('<%=txt_staffname.ClientID %>').value = "";
            document.getElementById('<%=txt_staffdept.ClientID %>').value = "";
            document.getElementById('<%=txt_desg.ClientID %>').value = "";
            document.getElementById('<%=txt_staff_type.ClientID %>').value = "";
            document.getElementById('<%=txt_staff_purpose.ClientID %>').value = "";
            document.getElementById('<%=txt_staff_ownvehilno.ClientID %>').value = "";
            document.getElementById('<%=txt_staff_owndrivername.ClientID %>').value = "";
            document.getElementById('<%=txt_staff_ownmobno.ClientID %>').value = "";
            document.getElementById('<%=txt_staff_vehilno.ClientID %>').value = "";
            document.getElementById('<%=txt_staff_route.ClientID %>').value = "";
            document.getElementById('<%=txt_staff_drop.ClientID %>').value = "";
            document.getElementById('<%=txt_staff_drvname.ClientID %>').value = "";
            document.getElementById('<%=txt_staff_mob.ClientID %>').value = "";
            document.getElementById('<%=imagestaff.ClientID %>').src = "";
            document.getElementById('<%=imagestaff.ClientID %>').value = "";
            staffcodemsg.innerHTML = "";
            //rdo_staff_in.checked = true;
           // rdo_staffapp_no.checked = true;
           // rdo_staff_other_trans.checked = true;
        }
        function prnoclear() {
            document.getElementById('<%=txt_studname.ClientID %>').value = "";
            document.getElementById('<%=txt_fname.ClientID %>').value = "";
            document.getElementById('<%=txt_fmobile.ClientID %>').value = "";
            document.getElementById('<%=txt_studtype1.ClientID %>').value = "";
            document.getElementById('<%=txt_pdegree.ClientID %>').value = "";
            document.getElementById('<%=txt_dept1.ClientID %>').value = "";
            document.getElementById('<%=txt_sem1.ClientID %>').value = "";
            document.getElementById('<%=txt_section.ClientID %>').value = "";
            document.getElementById('<%=image5.ClientID %>').src = "";
            document.getElementById('<%=image5.ClientID %>').value = "";
            document.getElementById('<%=image6.ClientID %>').src = "";
            document.getElementById('<%=image6.ClientID %>').value = "";
            document.getElementById('<%=image7.ClientID %>').src = "";
            document.getElementById('<%=image7.ClientID %>').value = "";
        }
        function parentclear() {
            var parent = document.getElementById('<%=div_parent.ClientID %>');
            parent.style.display = "block";
           // rb_adm_stud.checked = true;
           // rb_meetoffice.checked = true;
            document.getElementById('<%=txt_stud_rollno.ClientID %>').value = "";
            document.getElementById('<%=txt_studname.ClientID %>').value = "";
            document.getElementById('<%=txt_fname.ClientID %>').value = "";
            document.getElementById('<%=txt_fmobile.ClientID %>').value = "";
            document.getElementById('<%=txt_studtype1.ClientID %>').value = "";
            document.getElementById('<%=txt_purposevisit.ClientID %>').value = "";
            document.getElementById('<%=txt_pdegree.ClientID %>').value = "";
            document.getElementById('<%=txt_dept1.ClientID %>').value = "";
            document.getElementById('<%=txt_sem1.ClientID %>').value = "";
            document.getElementById('<%=txt_section.ClientID %>').value = "";
            document.getElementById('<%=txt_studrelation.ClientID %>').value = "";
            document.getElementById('<%=txt_staffname1.ClientID %>').value = "";
            document.getElementById('<%=txt_dpt.ClientID %>').value = "";
            document.getElementById('<%=txt_desgtn.ClientID %>').value = "";
            document.getElementById('<%=txt_meetstaffid.ClientID %>').value = "";
            document.getElementById('<%=txt_meetstaffname.ClientID %>').value = "";
            document.getElementById('<%=txt_meetstaffdept.ClientID %>').value = "";
            document.getElementById('<%=txt_meetstaffdesg.ClientID %>').value = "";
            document.getElementById('<%=txt_name1.ClientID %>').value = "";
            document.getElementById('<%=txt_relation.ClientID %>').value = "";
            document.getElementById('<%=txt_moblno.ClientID %>').value = "";
            document.getElementById('<%=txt_name2.ClientID %>').value = "";
            document.getElementById('<%=txt_addrs.ClientID %>').value = "";
            document.getElementById('<%=txt_pincode.ClientID %>').value = "";
            document.getElementById('<%=txt_city.ClientID %>').value = "";
            document.getElementById('<%=txt_district.ClientID %>').value = "";
            document.getElementById('<%=txt_state.ClientID %>').value = "";
            document.getElementById('<%=txt_mob.ClientID %>').value = "";
            document.getElementById('<%=txt_visit.ClientID %>').value = "";
            document.getElementById('<%=image5.ClientID %>').src = "";
            document.getElementById('<%=image5.ClientID %>').value = "";
            document.getElementById('<%=image6.ClientID %>').src = "";
            document.getElementById('<%=image6.ClientID %>').value = "";
            document.getElementById('<%=image7.ClientID %>').src = "";
            document.getElementById('<%=image7.ClientID %>').value = "";
            document.getElementById('<%=image8.ClientID %>').src = "";
            document.getElementById('<%=image8.ClientID %>').value = "";
            prnomsg.innerHTML = "";
        }
        function compdetclear() {
            document.getElementById('<%=txt_mno.ClientID %>').value = "";
          
            document.getElementById('<%=txt_str.ClientID %>').value = "";
            document.getElementById('<%=txt_cty.ClientID %>').value = "";
            document.getElementById('<%=txt_dis.ClientID %>').value = "";
            document.getElementById('<%=txt_stat.ClientID %>').value = "";
        }
        function companyclear() {
            document.getElementById('<%=txt_mno.ClientID %>').value = "";
          
            document.getElementById('<%=txt_str.ClientID %>').value = "";
            document.getElementById('<%=txt_cty.ClientID %>').value = "";
            document.getElementById('<%=txt_dis.ClientID %>').value = "";
            document.getElementById('<%=txt_stat.ClientID %>').value = "";
           // document.getElementById('<%=txt_compname.ClientID %>').value = "";
           var ms=document.getElementById('<%=txt_compname.ClientID %>').Text;
            document.getElementById('<%=txt_name4.ClientID %>').value = "";
            document.getElementById('<%=txt_desgn.ClientID %>').value = "";
            document.getElementById('<%=txt_dep.ClientID %>').value = "";
            document.getElementById('<%=txt_visit1.ClientID %>').value = "";
            var mssss=document.getElementById('<%=TextBox1.ClientID %>').Text;
         //  alert(mssss);
            document.getElementById('<%=sname.ClientID %>').value = "";
            document.getElementById('<%=txt_dpt1.ClientID %>').value = "";
            document.getElementById('<%=txt_desg1.ClientID %>').value = "";
            document.getElementById('<%=txt_type.ClientID %>').value = "";
            document.getElementById('<%=mblno.ClientID %>').value = "";
            document.getElementById('<%=txt_vehtype.ClientID %>').value = "";
            document.getElementById('<%=txt_vehno1.ClientID %>').value = "";
            document.getElementById('<%=txt_visitormeetstaffid.ClientID %>').value = "";
            document.getElementById('<%=txt_visitormeetstaffname.ClientID %>').value = "";
            document.getElementById('<%=txt_visitormeetstaffdept.ClientID %>').value = "";
            document.getElementById('<%=txt_visitormeetstaffdesg.ClientID %>').value = "";
            document.getElementById('<%=txt_visitormeetoffname.ClientID %>').value = "";
            document.getElementById('<%=txt_visitormeetoffdept.ClientID %>').value = "";
            document.getElementById('<%=txt_visitormeetoffdesg.ClientID %>').value = "";
            document.getElementById('<%=txt_visitormeetothername.ClientID %>').value = "";
            document.getElementById('<%=txt_visitormeetotherrel.ClientID %>').value = "";
            document.getElementById('<%=txt_visitormeetothermob.ClientID %>').value = "";
            document.getElementById('<%=image9.ClientID %>').src = "";
            document.getElementById('<%=image9.ClientID %>').value = "";
            document.getElementById('<%=image10.ClientID %>').src = "";
            document.getElementById('<%=image10.ClientID %>').value = "";
           // alert(mssss);
        }
        function vehidclear() {
            document.getElementById('<%=txt_rut.ClientID %>').value = "";
            document.getElementById('<%=txt_driver1.ClientID %>').value = "";
            document.getElementById('<%=txt_insurstatus.ClientID %>').value = "";
            document.getElementById('<%=txt_licstatus.ClientID %>').value = "";
            document.getElementById('<%=txt_fcstatus.ClientID %>').value = "";
        }
        function vehicleclear() {
            document.getElementById('<%=txt_vehicleno2.ClientID %>').value = "";
            document.getElementById('<%=txt_rut.ClientID %>').value = "";
            document.getElementById('<%=txt_driver1.ClientID %>').value = "";
            document.getElementById('<%=txt_insurstatus.ClientID %>').value = "";
            document.getElementById('<%=txt_licstatus.ClientID %>').value = "";
            document.getElementById('<%=txt_fcstatus.ClientID %>').value = "";
            document.getElementById('<%=txt_purpos1.ClientID %>').value = "";
            document.getElementById('<%=txt_personname.ClientID %>').value = "";
            document.getElementById('<%=txt_depart.ClientID %>').value = "";
            document.getElementById('<%=txt_design.ClientID %>').value = "";
            document.getElementById('<%=txt_vehicleno1.ClientID %>').value = "";
            document.getElementById('<%=txt_brotname.ClientID %>').value = "";
            document.getElementById('<%=txt_mblno1.ClientID %>').value = "";
            document.getElementById('<%=txt_purpose.ClientID %>').value = "";
            vehmsg.innerHTML = "";
        }
        function ponoclear() {
        
            var item = document.getElementById('<%=div_matitem.ClientID %>');
            item.style.display = "none";
            document.getElementById('<%=txt_suppliername.ClientID %>').value = "";
            document.getElementById('<%=txt_addr.ClientID %>').value = "";
            document.getElementById('<%=txt_pincode1.ClientID %>').value = "";
            document.getElementById('<%=txt_city1.ClientID %>').value = "";
            document.getElementById('<%=txt_dist.ClientID %>').value = "";
            document.getElementById('<%=txt_state1.ClientID %>').value = "";
            document.getElementById('<%=txt_contperson.ClientID %>').value = "";
            document.getElementById('<%=txt_name3.ClientID %>').value = "";
            document.getElementById('<%=txt_mobileno.ClientID %>').value = "";
        }
        function materialclear() {
            var item = document.getElementById('<%=div_matitem.ClientID %>');
            item.style.display = "none";
            document.getElementById('<%=txt_purordno.ClientID %>').value = "";
            document.getElementById('<%=txt_suppliername.ClientID %>').value = "";
            document.getElementById('<%=txt_addr.ClientID %>').value = "";
            document.getElementById('<%=txt_pincode1.ClientID %>').value = "";
            document.getElementById('<%=txt_city1.ClientID %>').value = "";
            document.getElementById('<%=txt_dist.ClientID %>').value = "";
            document.getElementById('<%=txt_state1.ClientID %>').value = "";
            document.getElementById('<%=txt_contperson.ClientID %>').value = "";
            document.getElementById('<%=txt_name3.ClientID %>').value = "";
            document.getElementById('<%=txt_mobileno.ClientID %>').value = "";
            document.getElementById('<%=txt_vehno.ClientID %>').value = "";
            document.getElementById('<%=txt_vehitype.ClientID %>').value = "";
            document.getElementById('<%=txt_drivname.ClientID %>').value = "";
            document.getElementById('<%=txt_mobno.ClientID %>').value = "";
            document.getElementById('<%=txt_vehino.ClientID %>').value = "";
            document.getElementById('<%=txt_vehitype1.ClientID %>').value = "";
            document.getElementById('<%=txt_bbyname.ClientID %>').value = "";
            document.getElementById('<%=txt_mobno1.ClientID %>').value = "";
            document.getElementById('<%=txt_itemname.ClientID %>').value = "";
            document.getElementById('<%=txt_qty.ClientID %>').value = "";
            document.getElementById('<%=txt_measure.ClientID %>').value = "";
            ponomsg.innerHTML = "";
        }
        function btnstudok() {
            validstud();
            if (statusmsg.innerHTML == "Approved") {
                return true;
                btntype=1;
            }
            else if (statusmsg.innerHTML == "Not Approved") {
                alert('Not Approved');
                statusmsg.innerHTML = "";
                studentclear();
                return false;
            }
            else {
                validstud();
                return false;
            }
        }
        function btnsmartok() {
            validsmart();
            if (statusmsg.innerHTML == "Approved") {
                return true;
                btntype=1;
            }
            else if (statusmsg.innerHTML == "Not Approved") {
                alert('Not Approved');
                statusmsg.innerHTML = "";
                studentclear();
                return false;
            }
            else {
                validsmart();
                return false;
            }
        }
        function btnstudclear() {
            studentclear();
          
            return false;
        }
        function btnstaffclear() {
            staffclear();
            return false;
        }
        function btnparentclear() {
            parentclear();
            return false;
        }
        function btncompanyclear() {
            companyclear();
            return false;
        }
        function btnmaterialclear() {
            materialclear();
            return false;
        }
        function btnvehicleclear() {
            vehicleclear();
            return false;
        }
        function btnerrorclose() {
            var studgo = document.getElementById('<%=imgdiv2.ClientID %>');
           // document.getElementById('<%=TextBox1.ClientID %>').value="";
            studgo.style.display = "none";
            return true;
        }
       
        function validstud() {
            var idval = "";
            var empty = "";
            idval = document.getElementById("<%= txt_rollno.ClientID %>").value;
            if (idval.trim() == "") {
                idval = document.getElementById("<%=txt_rollno.ClientID %>");
                idval.style.borderColor = 'Red';
                empty = "E";
            }
        }
        function validsmart() {
            var idval = "";
            var empty = "";
            idval = document.getElementById("<%= txt_smart.ClientID %>").value;
            if (idval.trim() == "") {
                idval = document.getElementById("<%=txt_smart.ClientID %>");
                idval.style.borderColor = 'Red';
                empty = "E";
            }
        }
        function validstaff() {
            var idval = "";
            var empty = "";
            idval = document.getElementById("<%=txt_staffid.ClientID %>").value;
            if (idval.trim() == "") {
                idval = document.getElementById("<%= txt_staffid.ClientID %>");
                idval.style.borderColor = 'Red';
                empty = "E";
            }
            if (empty.trim() != "") {
                return false;
            }
            else {
                return true;
                btntype=2;
            }
        }
        function validparent() {
            var idval = "";
            var empty = "";
            if (document.getElementById('<%=rb_adm_stud.ClientID %>').checked == true) {
                idval = document.getElementById("<%=txt_stud_rollno.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txt_stud_rollno.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";
                }
                idval = document.getElementById("<%=txt_studrelation.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txt_studrelation.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";
                }
                if (empty.trim() != "") {
                    return false;
                }
                else {
                    return true;
                    btntype=3;
                }
            }
            else if (document.getElementById('<%=rb_notadm_stud.ClientID %>').checked == true) {
                idval = document.getElementById("<%=txt_mob.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txt_mob.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";
                }
                idval = document.getElementById("<%=txt_name2.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txt_name2.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";
                }
                if (empty.trim() != "") {
                    return false;
                }
                else {
                    return true;
                }
            }
           
        }
        function validvisitor() {
            var idval = "";
            var empty = "";
            if(document.getElementById('<%=rb_company.ClientID %>').checked==true)
            {
                idval = document.getElementById("<%=txt_compname.ClientID %>").value;
            if (idval.trim() == "") {
                idval = document.getElementById("<%=txt_compname.ClientID %>");
                idval.style.borderColor = 'Red';
                empty = "E";
            }
             idval = document.getElementById("<%=txt_name4.ClientID %>").value;
            if (idval.trim() == "") {
                idval = document.getElementById("<%=txt_name4.ClientID %>");
                idval.style.borderColor = 'Red';
                empty = "E";
            }
             idval = document.getElementById("<%=txt_mno.ClientID %>").value;
            if (idval.trim() == "") {
                idval = document.getElementById("<%=txt_mno.ClientID %>");
                idval.style.borderColor = 'Red';
                empty = "E";
            }
            }
            else if(document.getElementById('<%=rb_individual.ClientID %>').checked==true)
            {
            idval = document.getElementById("<%=txt_name4.ClientID %>").value;
            if (idval.trim() == "") {
                idval = document.getElementById("<%=txt_name4.ClientID %>");
                idval.style.borderColor = 'Red';
                empty = "E";
            }
            idval = document.getElementById("<%=txt_mno.ClientID %>").value;
            if (idval.trim() == "") {
                idval = document.getElementById("<%=txt_mno.ClientID %>");
                idval.style.borderColor = 'Red';
                empty = "E";
            }
            }
            if (empty.trim() != "") {
                return false;
            }
            else {
                return true;
                btntype=4;
            }
        }
        function validmaterial() {
            var idval = "";
            var empty = "";
            var rb_ordmaterial=document.getElementById("<%=rb_ordmaterial.ClientID %>").value;
             var rb_other=document.getElementById("<%=rb_other.ClientID %>").value;
              var rb_service=document.getElementById("<%=rb_service.ClientID %>").value;
            
            if (rb_ordmaterial.checked == true) {
                idval = document.getElementById("<%=txt_purordno.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txt_purordno.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";
                }
                if (empty.trim() != "") {
                    return false;
                }
                else {
                    return true;
                    btntype=5;
                }
            }
            else if (rb_other.checked == true) {
                idval = document.getElementById("<%=txt_itemname.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txt_itemname.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";
                }
                idval = document.getElementById("<%=txt_qty.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txt_qty.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";
                }
                if (empty.trim() != "") {
                    return false;
                }
                else {
                    return true;
                }
            }
            else if (rb_service.checked == true) {
            }
        }
        function validvehicle() {
            var idval = "";
            var empty = "";
            if (document.getElementById('<%=rb_instuveh.ClientID %>').checked == true) {
                idval = document.getElementById("<%=txt_vehicleno2.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txt_vehicleno2.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";
                }
                if (empty.trim() != "") {
                    return false;
                }
                else {
                    return true;
                    btntype=6;
                }
            }
            else if (document.getElementById('<%=rb_otherveh.ClientID %>').checked == true) {
                idval = document.getElementById("<%=txt_vehicleno1.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txt_vehicleno1.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";
                }
                if (empty.trim() != "") {
                    return false;
                }
                else {
                    return true;
                }
            }
        }
        function myFunction(x) {
            x.style.borderColor = "#c4c4c4";
        }
        <%--Student tab time--%>
        setInterval(function () {
           // document.getElementById("<%=txt_time.ClientID %>").value = (new Date()).toLocaleTimeString();
            return false;
        }, 1000);
         <%--Staff tab time--%>
        setInterval(function () {
            document.getElementById("<%=txt_staff_time.ClientID %>").value = (new Date()).toLocaleTimeString();
            return false;
        }, 1000);
          <%--Parents tab time--%>
        setInterval(function () {
            document.getElementById("<%=txt_partime.ClientID %>").value = (new Date()).toLocaleTimeString();
            return false;
        }, 1000);
         <%--Visitor tab time--%>
        setInterval(function () {
        //magesh 5.6.18
        
         //  document.getElementById("<%=txt_visittime.ClientID %>").value = (new Date()).toLocaleTimeString();
            return false;
        }, 1000);
          <%--Material tab time--%>
        setInterval(function () {
            document.getElementById("<%=txt_materialtime.ClientID %>").value = (new Date()).toLocaleTimeString();
            return false;
        }, 1000);
        <%--Vehicle tab time--%>
        setInterval(function () {
            document.getElementById("<%=txt_vehicletime.ClientID %>").value = (new Date()).toLocaleTimeString();
            return false;
        }, 1000);
        function myFunCaps(id)
        {
        var txt  = document.getElementById(id);
        var value=txt.value;
        txt.value= value.charAt(0).toUpperCase()+value.substr(1);
        }
   function setFocusToTextBox(){
    var textbox = document.getElementById("<%=txt_staff_smartcard.ClientID %>");
    textbox.focus();
    //textbox.scrollIntoView();
}



  function checkDate() {
            var fromDate = "";
            var toDate = "";
            var date = ""
            var date1 = ""
            var month = "";
            var month1 = "";
            var year = "";
            var year1 = "";
            var empty = "";
            fromDate = document.getElementById('<%=txt_fromdate.ClientID%>').value;
            toDate = document.getElementById('<%=txt_todate.ClientID%>').value;

            date = fromDate.substring(0, 2);
            month = fromDate.substring(3, 5);
            year = fromDate.substring(6, 10);

            date1 = toDate.substring(0, 2);
            month1 = toDate.substring(3, 5);
            year1 = toDate.substring(6, 10);
            var today = new Date();
            //  var currentDate = today.getDate() + '/' + (today.getMonth() + 1) + '/' + today.getFullYear();
            var today = new Date();
            var dd = today.getDate();
            var mm = today.getMonth() + 1;
            var yyyy = today.getFullYear();
            if (dd < 10) { dd = '0' + dd }
            if (mm < 10) { mm = '0' + mm }
            var today = dd + '/' + mm + '/' + yyyy;

            if (year == year1) {
                if (month == month1) {
                    if (date == date1) {
                        empty = "";
                    }
                    else if (date < date1) {
                        empty = "";
                    }
                    else {
                        empty = "e";
                    }
                }
                else if (month < month1) {
                    empty = "";
                }
                else if (month > month1) {
                    empty = "e";
                }
            }
            else if (year < year1) {
                empty = "";
            }
            else if (year > year1) {
                empty = "e";
            }
            if (empty != "") {
                document.getElementById('<%=txt_fromdate.ClientID%>').value = today;
                document.getElementById('<%=txt_todate.ClientID%>').value = today;
                alert("To date should be greater than from date ");
                return false;
            }
        }
       

          
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>    
    <div>
        <center>
            <span class="fontstyleheader" style="color: #008000; margin:0px; margin-bottom:10px; margin-top:10px; position:relative;">Gate Pass Entry/Exit</span>
        </center>
    </div>    
    <center>   
        <div class="maindivstyle maindivstylesize">
            <div>
                <br />
                 <table class="table" style="width:900px; ">
                    <tr>
                        <td id="studenttd" runat="server" >
                            <asp:ImageButton ID="imgbtn_student" runat="server" Width="50px" Height="50px" ImageUrl="~/Hostel Gete Images/download_stud.jpg"
                                OnClientClick="return checkmain()" /><br />
                            <asp:Label ID="Label12" runat="server" Style="top: 10px; left: 6px;" Text="Student"></asp:Label>
                            <%-- <asp:RadioButton ID="rb_student" runat="server" Text="Student" AutoPostBack="true"
                                OnCheckedChanged="rb_student_CheckedChanged" GroupName="r1"></asp:RadioButton>--%>
                        </td>
                        <td id="stafftd" runat="server" >
                            <%--<asp:RadioButton ID="rb_staff" runat="server" Text="Staff" AutoPostBack="true" GroupName="r1"
                                OnCheckedChanged="rb_staff_CheckedChanged"></asp:RadioButton>--%>
                            <asp:ImageButton ID="imgbtn_staff" runat="server" ImageUrl="~/Hostel Gete Images/download_staff2.jpg"
                                Width="50px" Height="50px" OnClientClick="return staffbtn()"/>
                            <br />
                            <asp:Label ID="Label13" runat="server" Style="top: 10px; left: 6px;" Text="Staff"></asp:Label>
                        </td>
                        <td id="parenttd" runat="server" >
                            <%--<asp:RadioButton ID="rb_parents" runat="server" Text="Parents" AutoPostBack="true"
                                OnCheckedChanged="rb_parents_CheckedChanged" GroupName="r1"></asp:RadioButton>--%>
                            <asp:ImageButton ID="imgbtn_parents" runat="server" ImageUrl="~/Hostel Gete Images/download_parents1.jpg"
                                Width="50px" Height="50px" OnClientClick="return parentsbtn();"/><br />
                            <asp:Label ID="Label14" runat="server" Style="top: 10px; left: 6px;" Text="Parents"></asp:Label>
                        </td>
                        <td id="visitortd" runat="server" >
                            <%--<asp:RadioButton ID="rb_visitor" runat="server" Text="Visitor" AutoPostBack="true"
                                OnCheckedChanged="rb_visitor_CheckedChanged" GroupName="r1"></asp:RadioButton>--%>
                            <asp:ImageButton ID="imgbtn_visitor" runat="server" ImageUrl="~/request_img/visit.jpg" AutoPostBack="true" OnClick="imgbtn_visitor1_Click"
                     OnClientClick="return visitorbtn();" Width="50px" Height="50px" />
                     <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="getcomppername" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_name4"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="panelbackground">
                            </asp:AutoCompleteExtender>
                            <br />
                            <asp:Label ID="Label15" runat="server" Style="top: 10px; left: 6px;" Text="Visitor"></asp:Label>
                            <input type="hidden" runat="server" id="hid" />
                            <input type="hidden" runat="server" id="Hiddcen1" />
                        </td>
                        <td id="materialtd" runat="server"  >
                            <%-- <asp:RadioButton ID="rb_material" runat="server" Text="Material" AutoPostBack="true"
                                OnCheckedChanged="rb_material_CheckedChanged" GroupName="r1"></asp:RadioButton>--%>
                            <asp:ImageButton ID="imgbtn_material" runat="server" ImageUrl="~/Hostel Gete Images/purchase.jpg"
                                OnClientClick="return materialbtn();" Width="50px" Height="50px"  /><br />
                            <asp:Label ID="Label16" runat="server" Style="top: 10px; left: 6px;" Text="Material"></asp:Label>
                        </td>
                        <td id="vehicletd" runat="server"  >
                            <%-- <asp:RadioButton ID="rb_vehicle" runat="server" Text="Vehicle" AutoPostBack="true"
                                OnCheckedChanged="rb_vehicle_CheckedChanged" GroupName="r1"></asp:RadioButton>--%>
                            <asp:ImageButton ID="imgbtn_vehicle" runat="server" ImageUrl="~/Hostel Gete Images/bus.png"
                                OnClientClick="return vehiclebtn();" Width="50px" Height="50px"  /><br />
                            <asp:Label ID="Label17" runat="server" Style="top: 10px; left: 6px;" Text="Vehicle"></asp:Label>
                        </td>
                    </tr>
                </table>
                <br />
            </div>     
              
            <center>
            <asp:Label ID="lblUserCode" runat="server" style="display: none;" ></asp:Label>
                <div id="div_student" runat="server" style="display: none;">
                    <%--div for In & Out , Date & Time--%>
                    <table width="900px">
                        <tr>
                            <td align="right">
                                <div class="maindivstyle" align="center" style="border-radius: 7px; width: 150px;
                                    height: 30px;">
                                    <asp:RadioButton ID="rb_in" runat="server" RepeatDirection="Horizontal" GroupName="same"
                                        Text="Out" Font-Size="Larger"   AutoPostBack="true"  Checked="true" OnCheckedChanged="rb_out_CheckedChanged"></asp:RadioButton>
                                    <asp:RadioButton ID="rb_out" runat="server" RepeatDirection="Horizontal" GroupName="same"
                                        Text="In"  Font-Size="Larger"  AutoPostBack="true" OnCheckedChanged="rb_in_CheckedChanged"></asp:RadioButton>&nbsp;&nbsp;
                                    
                                </div>
                            </td>
                            <td align="right">
                                <div class="maindivstyle" align="center" style="border-radius: 7px; width: 300px;
                                    height: 40px;">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_date" runat="server" Style="top: 10px; left: 6px;" Text="Date"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="Updp_date" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txt_date" runat="server" CssClass="txtcaps txtheight" OnTextChanged="txt_date_TextChanged"
                                                            ReadOnly="true" AutoPostBack="true"></asp:TextBox>
                                                        <%--<asp:CalendarExtender ID="Cal_date" TargetControlID="txt_date" runat="server" CssClass="cal_Theme1 ajax__calendar_active"
                                                            Format="dd/MM/yyyy">--%>
                                                      <%--  </asp:CalendarExtender>--%>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_time" runat="server" Style="top: 10px; left: 6px;" Text="Time"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_time" runat="server" CssClass="txtcaps txtheight" OnTextChanged="txt_time_TextChanged"
                                                    AutoPostBack="true"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <table class="" width="98%" height="100%">
                    <tr>
                    <td colspan="5">
                     <div id="Div1" runat="server" visible="false" style="height: 100%; z-index: 100px; 
            width: 100%; position: absolute; margin-top:50px; 
            left: 0%;">
            <center>
                    <div id="Div2" runat="server" class="table" style="background-color: White; margin-top: -63px;margin-left: -173px;
                        height: 100px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                         border-radius: 10px;">
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="Label3" runat="server" Text="Password" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                             <asp:TextBox ID="TextBox3" runat="server"  CssClass="txtcaps txtheight3"  TextMode="Password"></asp:TextBox>
                                              <asp:Label ID="Label4" runat="server" Text="Please Enter Correct Password" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="Btnn2" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                width: 65px;" Text="Ok" runat="server" OnClick="Btnn2_Click"  />
                                                 <asp:Button ID="Bon2" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                width: 65px;" Text="Close" runat="server" OnClick="Bon2_Click"  />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                    </div>
                    </center>
            </div>
            </td>
                    </tr>
                    <tr>
                    <td>
                              <asp:Label ID="Label1" runat="server" style="top:10px;" Text="Device Id" Visible="false"></asp:Label>
                            </td>
                            <td align="left">
                             <asp:UpdatePanel ID="updPnlTmr" runat="server">
                <ContentTemplate>
                    <asp:Timer ID="tmrTTStat" runat="server" Interval="5000" OnTick="tmrTTStat_OnTick">
                    </asp:Timer>
                             <asp:TextBox ID="TextBox2" runat="server"  CssClass="txtcaps txtheight3"   onkeypress="return enterscript(event)"  onkeyup="return getsmart(this)" Visible="false"></asp:TextBox>
                           <%--  <span style="color: Red;">*</span>--%>
                                <span id="Span1"></span></ContentTemplate></asp:UpdatePanel>
                            </td>
                    </tr>
                    <tr>
                     <td>
                              <asp:Label ID="lbl_smart" runat="server" style="top:10px;" Text="SmartCard No"></asp:Label>
                            </td>
                            <td align="left">
                             <asp:TextBox ID="txt_smart" runat="server"  CssClass="txtcaps txtheight3" TextMode="Password"  onkeypress="return enterscript(event)"  onkeyup="return getsmart(this)"></asp:TextBox>
                             <span style="color: Red;">*</span>
                                <span id="smarterr"></span>
                            </td>
                            <td>
                              <asp:Label ID="Ladept" runat="server" style="top:10px;" Text="Department" ></asp:Label>
                            </td>
                            <td align="left">
                             <asp:TextBox ID="Txtde" runat="server"  CssClass="txtcaps txtheight3"  onchange="return getsmartr(this)"></asp:TextBox>
                               <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="getdept" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="Txtde"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="panelbackground">
                                </asp:AutoCompleteExtender>
                            
                            </td>
                    </tr>
                        <tr>
                            <%--Rollno--%>
                            <td>
                                <asp:Label ID="lbl_rollno" runat="server" Style="top: 10px;" Text="Roll No"></asp:Label>
                            </td>
                            <td align="left">
                             <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                                <asp:TextBox ID="txt_rollno" runat="server" AutoPostBack="true" CssClass="txtcaps txtheight2"  OnTextChanged="txt_rollno_OnTextChanged"
                                    onfocus="return myFunction(this)" onchange="return getsmartrollno(this)" ></asp:TextBox>
                                    <span style="color: Red;">*</span>
                                <span id="rnomsg"></span>
                                <asp:FilteredTextBoxExtender ID="ftext_rollno" runat="server" TargetControlID="txt_rollno"  FilterType="UppercaseLetters,LowercaseLetters,numbers,custom"
                                  ValidChars=" ">
                                </asp:FilteredTextBoxExtender>
                                <asp:AutoCompleteExtender ID="acext_rollno" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_rollno"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="panelbackground">
                                </asp:AutoCompleteExtender>
                                </ContentTemplate></asp:UpdatePanel>
                            </td>
                            <%--StudType--%>
                            <td>
                                <asp:Label ID="lbl_studtype" runat="server" Style="top: 10px; left: 6px;" Text="Student Type"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_studtype" runat="server" CssClass="txtcaps txtheight2" BackColor="#ffffcc"></asp:TextBox>
                            </td>
                            <%--StudImage--%>
                            <td rowspan="3">
                                <asp:Image ID="image2" runat="server" ToolTip="Student's Photo" ImageUrl="" Style="height: 110px; width: 130px;" />
                            </td>
                        </tr>
                        <tr>
                            <%--StudName--%>
                            <td>
                                <asp:Label ID="lbl_name" runat="server" Style="top: 10px; left: 6px;" Text="Name"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_name" runat="server" CssClass="txtcaps txtheight2" Width="250px"                                   
                                    onfocus="return myFunction(this)" OnTextChanged="txt_name_TextChanged" AutoPostBack="true" onblur="return visitorcompdet(this.value)"></asp:TextBox>
                               <%--<%-- <asp:FilteredTextBoxExtender ID="ftext_name" runat="server" TargetControlID="txt_name"
                                    ><%--FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-&()"
                                </asp:FilteredTextBoxExtender>--%>
                                 <asp:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="namede" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_name"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="panelbackground">
                                </asp:AutoCompleteExtender>
                            </td>
                            <%--Degree--%>
                            <td>
                                <asp:Label ID="lbl_degree" runat="server" Style="top: 10px; left: 6px;" Text="Degree"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_degree" runat="server" CssClass="txtcaps txtheight2" BackColor="#ffffcc"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <%--Approval Status--%>
                            <td>
                                <asp:Label ID="lbl_approval" runat="server" Style="top: 10px; left: 6px;" Text="Approval Status "></asp:Label>
                            </td>
                            <td>
                                <span id="statusmsg" style="font-size:large ;font-weight:bold ;" ></span>
                            </td>
                            <%--Department--%>
                            <td>
                                <asp:Label ID="lbl_dept" runat="server" Style="top: 10px; left: 6px;" Text="Department"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_dept" runat="server" Width="200px" CssClass="txtcaps txtheight2"
                                    BackColor="#ffffcc"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <%--ApproveDate & Time--%>
                            <td>
                                <asp:Label ID="lbl_apdate" runat="server" Style="top: 10px; left: 6px;" Text="Exit Date & Time"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_apdate" runat="server" CssClass="txtcaps txtheight" BackColor="#ffffcc">
                                </asp:TextBox>
                                &nbsp;
                                <asp:TextBox ID="txt_aptime" runat="server" CssClass="txtcaps txtheight" BackColor="#ffffcc"></asp:TextBox>
                            </td>
                            <%--Semester & Sections--%>
                            <td>
                                <asp:Label ID="lbl_sem" runat="server" Style="top: 10px; left: 6px;" Text="Semester"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txt_sem" runat="server" CssClass="txtcaps" Width="50px" BackColor="#ffffcc"></asp:TextBox>
                                <asp:Label ID="lbl_sec" runat="server" Style="top: 10px; left: 6px;" Text="Section"></asp:Label>
                                <asp:TextBox ID="txt_sec" runat="server" CssClass="txtcaps txtheight2" Width="50px"
                                    BackColor="#ffffcc"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <%--ExpectedDate--%>
                            <td>
                                <asp:Label ID="lbl_expdate" runat="server" Style="top: 10px; left: 6px;" Text="Expected Date&Time"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_expdate" runat="server" CssClass="txtcaps txtheight" OnTextChanged="txt_expdate_TextChanged"
                                            BackColor="#ffffcc"></asp:TextBox>
                                            &nbsp;
                            <%--ExpectedTime--%>
                                <asp:TextBox ID="txt_exptime" runat="server" CssClass="txtcaps txtheight" BackColor="#ffffcc"></asp:TextBox>
                            </td>
                            <%--Approved staff--%>
                            <td>
                                <asp:Label ID="lbl_apstaff" runat="server" Style="top: 10px; left: 6px;" Text="Staff"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_apstaff" runat="server" CssClass="txtcaps" Width="200px" BackColor="#ffffcc" onblur="getstaffdet(this.value)"></asp:TextBox>
                                <%--<asp:FilteredTextBoxExtender ID="ftext_apstaff" runat="server" TargetControlID="txt_apstaff"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                </asp:FilteredTextBoxExtender>
                                <asp:AutoCompleteExtender ID="acext_apstaff" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="getstaffnamewithdept" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_apstaff"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="panelbackground">
                                </asp:AutoCompleteExtender>--%>
                            </td>
                            <%--Staff Image--%>
                            <td rowspan="3">
                                <asp:Image ID="image4" runat="server" ToolTip="Approved Staff Photo" ImageUrl="" Style="height: 110px; width: 130px;" />
                            </td>
                        </tr>
                        <tr>
                           <%--Purpose--%>
                            <td>
                                <asp:Label ID="lbl_purpose1" runat="server" Style="top: 10px; left: 6px;" Text="Purpose/Leave Reason"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_purpose1" runat="server" CssClass="txtcaps txtheight5" onkeyup="myFunCaps(this.id)"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="ftext_purpose1" runat="server" TargetControlID="txt_purpose1"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .,-&">
                                </asp:FilteredTextBoxExtender>
                            </td>
                            <%--Approved Staff Department--%>
                            <td>
                                <asp:Label ID="lbl_apdept" runat="server" Style="top: 10px; left: 6px;" Text="Department"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_apdept" runat="server" CssClass="txtcaps" Width="200px" BackColor="#ffffcc"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <%--Approved Staff Designation--%>
                            <td>
                                <asp:Label ID="lbl_apdesgn" runat="server" Style="top: 10px; left: 6px;" Text="Designation"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_apdesgn" runat="server" CssClass="txtcaps txtheight4" BackColor="#ffffcc"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <%--EntryExit--%>
                            <td>
                                <asp:Label ID="lbl_entryby" runat="server" Style="top: 10px; left: 6px;" Text="Entry/Exit By"></asp:Label>
                            </td>
                            <td colspan="4">
                               <asp:RadioButton ID="rb_inst" runat="server" RepeatDirection="Horizontal" GroupName="r3"
                                    Text="Institution Transport" onchange="return checkrbowninst();" OnCheckedChanged="rb_inst_CheckedChanged">
                                </asp:RadioButton>
                                <asp:RadioButton ID="rb_own" runat="server" RepeatDirection="Horizontal" GroupName="r3"
                                    Text="Own Transport" OnCheckedChanged="rb_own_CheckedChanged" onchange="return checkrbowninst();">
                                </asp:RadioButton>
                                <asp:RadioButton ID="rb_vehother" runat="server" RepeatDirection="Horizontal" GroupName="r3"
                                    Text="Others" Checked="true" onchange="return checkrbowninst();" OnCheckedChanged="rb_vehother_CheckedChanged">
                                </asp:RadioButton>
                            </td>
                        </tr>
                        
                                   

                        <tr>
                        </tr>
                        <tr>
                            <%--EntryExit Own Vehicle--%>
                            <td colspan="4">
                                <div id="div_entryexitown" runat="server" style="display: none;">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_ownvehno" runat="server" Style="top: 10px; left: 6px;" Text="Vehicle No"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_ownvehno" runat="server" CssClass="txtcaps txtheight" MaxLength="15"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="ftext_ownvehno" runat="server" TargetControlID="txt_ownvehno"
                                    FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" -">
                                </asp:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_owndrivername" runat="server" Style="top: 10px; left: 6px;" Text="Driver Name"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_owndrivername" runat="server" CssClass="txtcaps txtheight2"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="ftext_owndrivername" runat="server" TargetControlID="txt_owndrivername"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                </asp:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_ownmob" runat="server" Style="top: 10px; left: 6px;" Text="Mobile No"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_ownmob" runat="server" CssClass="txtcaps txtheight" MaxLength="10"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="ftext_ownmob" runat="server" TargetControlID="txt_ownmob"
                                    FilterType="numbers" ValidChars="">
                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <%--EntryExit InstituteVehicle--%>
                            <td colspan="5">
                                <div id="div_entryexit" runat="server" style="display: none;">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_vehicleno" runat="server" Style="top: 10px; left: 6px;" Text="Vehicle No"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_vehicleno" runat="server" CssClass="txtcaps txtheight" onblur="getdriverdet(this.value)" MaxLength="15"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="ftext_vehicleno" runat="server" TargetControlID="txt_vehicleno"
                                                    FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" .-&">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:AutoCompleteExtender ID="acext_vehicleno" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="getvehicle" MinimumPrefixLength="0" CompletionInterval="100"
                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_vehicleno"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    CompletionListItemCssClass="panelbackground">
                                                </asp:AutoCompleteExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_drivername" runat="server" Style="top: 10px; left: 6px;" Text="Driver Name"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_drivername" runat="server" CssClass="txtcaps txtheight2" BackColor="#ffffcc"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_route" runat="server" Style="top: 10px; left: 6px;" Text="Route"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_route" runat="server" CssClass="txtcaps txtheight" BackColor="#ffffcc"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_mobile" runat="server" Style="top: 10px; left: 6px;" Text="Mobile No" MaxLength="10"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_mobile" runat="server" CssClass="txtcaps txtheight" BackColor="#ffffcc"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_drop" runat="server" Style="top: 10px; left: 6px;" Text="Drop Stage"></asp:Label>
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txt_drop" runat="server" CssClass="txtcaps txtheight4"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="acext_route" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="Getstage" MinimumPrefixLength="0" CompletionInterval="100"
                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_drop"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    CompletionListItemCssClass="panelbackground">
                                                </asp:AutoCompleteExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>

                           <tr>
                            <%--Leave Details--%>
                            <td>
                                <asp:Label ID="lbl_leave" runat="server" Style="top: 10px; left: 6px;" Text="Leave Details"></asp:Label>
                            </td>
                            <td>
                             <asp:Label ID="lbl_from" runat="server" Style="top: 10px; left: 6px;" Text="From:"></asp:Label>
                              
                         
                                      <asp:TextBox ID="txt_fromdate" runat="server" Style="height: 20px; width: 75px;"
                                                                    onchange="return checkDate()"></asp:TextBox>
                                                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_fromdate" runat="server"
                                                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                                </asp:CalendarExtender>
                                </td>
                          
                            <td colspan="4">
                                <asp:Label ID="lbl_to" runat="server" Style="top: 10px; left: 6px;" Text="To:"></asp:Label>
                            
                         
                                   <asp:TextBox ID="txt_todate" runat="server" Style="height: 20px; width: 75px;" onchange="return checkDate()"></asp:TextBox>
                                                                <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_todate" runat="server"
                                                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                                </asp:CalendarExtender>
                                </td>
                            
                        </tr>
                        <tr>
                            
                                <asp:Label ID="lbl_error1" Visible="false" runat="server" ForeColor="Red"></asp:Label>
                               
                      
                        </tr>
                          <tr>
                            <td>
                            </td>
                            <td>
                             <asp:Label ID="lbl_leatype" runat="server" Style="top: 10px; left: 6px;" Text="Leave Type:"></asp:Label>
                              
                           
                             <asp:TextBox ID="txt_leatype" runat="server" CssClass="txtcaps txtheight2">
                                   </asp:TextBox>
                                   </td>
                                   <td>
                                    <asp:Button ID="Butngo" runat="server" CssClass="textbox btn2" Text="Pervious Leave Details" Width="149px" Height="34px"  OnClick="Butngo_Click" BackColor="#00dbff"  />
                    </td>
                          <%--</td>
                            <td colspan="5">
                                <asp:Label ID="lbl_leareason" runat="server" Style="top: 10px; left: 6px;" Text="Leave Reason:"></asp:Label>
                              <asp:TextBox ID="txt_leareason" runat="server" CssClass="txtcaps txtheight2">
                                   </asp:TextBox>
                          
                          </td>--%>
                        </tr>
                          <tr>
                          
                            <td>
                                  <asp:Label ID="lbl_preleave" runat="server" Style="top: 10px; left: 6px;" Text="Previous Leaves"></asp:Label>
                            </td>
                            </tr>
                          

                            <tr>
                            <td>
                              <asp:Label ID="lbl_regvisit" runat="server" Style="top: 10px; left: 6px;" Text="Registered Visitors"></asp:Label>
                            
                            </td>
                            </tr>
                            <tr>
                            <td>
                            
                            </td>
                             <td colspan="10">
                                
                                <asp:Image ID="imageregvisitfar" runat="server" ToolTip="regvisit1 Photo" ImageUrl="" Style="height: 110px; width: 130px;" />

                                 <asp:ImageButton ID="ImageButton3" Visible="true" runat="server" Width="40px" Height="40px" ImageUrl="~/image/okimg.jpg"
                Style="height: 30px; width: 30px; position: absolute; margin-top: -11px; margin-left: -44px;"
                OnClick="btn_Question_Bank_popup3_Click" />
                                
                           
                                <asp:Image ID="imageregvisitmon" runat="server" ToolTip="regvisit2 Photo" ImageUrl="" Style="height: 110px; width: 130px;" />
                                 <asp:ImageButton ID="ImageButton1" Visible="true" runat="server" Width="40px"  Height="40px" ImageUrl="~/image/okimg.jpg"
                Style="height: 30px; width: 30px; position: absolute; margin-top: -11px; margin-left: -34px;"
                OnClick="btn_Question_Bank_popup_Click"  />
                           
                                <asp:Image ID="imageregvisitgaur" runat="server" ToolTip="regvisit3 Photo" ImageUrl="" Style="height: 110px; width: 130px;" />
                                <asp:ImageButton ID="ImageButton2" Visible="true" runat="server" Width="40px" Height="40px" ImageUrl="~/image/okimg.jpg"
                Style="height: 30px; width: 30px; position: absolute; margin-top: -11px; margin-left: -24px;"
                OnClick="btn_Question_Bank_popup1_Click" />
                           
                            </td>
                            </tr>
                    </table>
                    <asp:Button ID="btn_ok" runat="server" CssClass="textbox btn2" Text="Save"  OnClientClick="return btnstudok() 
btnsmartok();" OnClick="btn_ok_Click" Visible="false"  />
 <asp:Button ID="Btnexit" runat="server" CssClass="textbox btn2" Text="Exit"   OnClick="Btnexit_Click" Visible="True"  />
                    <asp:Button ID="btn_clear" runat="server" CssClass="textbox btn2" Text="Clear" OnClientClick="return btnstudclear();" />
                    <br />
                       <br />
                          <br />
                             <br />
                             <center>
                        <div id="div4" runat="server" visible="false" style="width: 923px; height: 350px;
                            background-color: White;" class="spreadborder">
                            <br />
                            <FarPoint:FpSpread ID="Fpspread2" runat="server" Visible="false" BorderStyle="NotSet"
                                BorderWidth="0px" ActiveSheetViewIndex="0">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                            
                        </div>
                        </center>
                         
                </div>
            </center>
            <center>
            <div id="div_staff" runat="server"  style="display: none; ">
                <table class="" width="98%">
                    <tr>
                        <td align="right">
                            <div class="maindivstyle" align="center" style="border-radius: 7px; width: 150px;
                                height: 30px;">
                                <asp:RadioButton ID="rdo_staff_in" runat="server" RepeatDirection="Horizontal" GroupName="staffin"
                                    Text="In" Checked="true" Font-Size="Larger" onchange="return staffinoutclr();" OnCheckedChanged="rdo_staff_in_CheckedChanged" AutoPostBack="true"></asp:RadioButton>
                                &nbsp;&nbsp;
                                <asp:RadioButton ID="rdo_staff_out" runat="server" RepeatDirection="Horizontal" GroupName="staffin"
                                    Text="Out" Font-Size="Larger" onchange="return staffinoutclr();" OnCheckedChanged="rdo_staff_out_CheckedChanged" AutoPostBack="true"></asp:RadioButton>
                            </div>
                        </td>
                        <td align="right">
                            <div class="maindivstyle" align="center" style="border-radius: 7px; width: 300px;
                                height: 40px;">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_staffdate" runat="server" Style="top: 10px; left: 6px;" Text="Date"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="Upp2" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_staff_date" runat="server" CssClass="txtcaps textbox1 txtheight"
                                                        OnTextChanged="txt_satffdate_TextChanged" ReadOnly="true" AutoPostBack="true"></asp:TextBox>
                                                    <%--<asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_staff_date" runat="server"
                                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                    </asp:CalendarExtender>--%>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_staff_time" runat="server" Style="top: 10px; left: 6px;" Text="Time"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_staff_time" runat="server" CssClass="textbox textbox1 txtheight"
                                                OnTextChanged="txt_stafftime_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                 <table class="" width="98%">
                 <tr>
                        <%--Staff code --%>
                         <td>
                            <asp:Label ID="lbl_staff_smartcard" runat="server" Style="top: 10px; left: 6px;" Text="SmartCard No" Width="110px"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_staff_smartcard" runat="server" TextMode="Password"  onkeypress="return enterscript(event)"  onkeyup="return getstaffbysmartcard(this)" CssClass="txtcaps textbox1 txtheight2"
                               onfocus="return myFunction(this)"></asp:TextBox>                                          
                        </td>
                        </tr>
                        <tr>
                        <td>
                            <asp:Label ID="lbl_staffid" runat="server" Style="top: 10px; left: 6px;" Text="Staff Code"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_staffid" runat="server" CssClass="txtcaps textbox1 txtheight2"
                               onchange="return checkstaffcode(this.value)" onkeyup="return checkstaffcode(this.value)" onblur="getstaffdetbyid(this.value)" onfocus="return myFunction(this)"></asp:TextBox>
                                <span style="color: Red;">*</span>
                            <span id="staffcodemsg"></span>
                            <asp:AutoCompleteExtender ID="acext_staffid" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="getstaffcode" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_staffid"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="panelbackground">
                            </asp:AutoCompleteExtender>
                        </td>
                        <%--Staff department --%>
                        <td>
                            <asp:Label ID="lbl_staffdept" runat="server" Style="top: 10px; left: 6px;" Text="Department"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_staffdept" runat="server" Width="200px" CssClass="txtcaps textbox1" BackColor="#ffffcc"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <%--Staff Name--%>
                        <td>
                            <asp:Label ID="lbl_staffname" runat="server" Style="top: 10px; left: 6px;" Text="Name"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_staffname" runat="server" Width="250px" CssClass="txtcaps textbox1 txtheight2"
                                onblur="getstaffdetbyname(this.value)"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="ftext_staffname" runat="server" TargetControlID="txt_staffname"
                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-&()">
                            </asp:FilteredTextBoxExtender>
                            <asp:AutoCompleteExtender ID="acext_staffname" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="getstaffnamewithdept" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_staffname"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="panelbackground">
                            </asp:AutoCompleteExtender>
                        </td>
                        <%--Staff Designation--%>
                        <td>
                            <asp:Label ID="lbl_desg" runat="server" Style="top: 10px; left: 6px;" Text="Designation"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_desg" runat="server" CssClass="txtcaps textbox1" Width="200px" BackColor="#ffffcc"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                        <%--Staff Image--%>
                        <td rowspan="3">
                            <asp:Image ID="imagestaff" runat="server" ImageUrl="" ToolTip="Staff Photo" Style="height: 110px; width: 130px;"/>
                        </td>
                    </tr>
                    <tr>
                    <%--Staff Approval Status--%>
                        <td>
                            <asp:Label ID="lbl_staff_appvl" runat="server" Visible="true" Style="top: 10px; left: 6px;" Text="Approval Status"></asp:Label>
                        </td>
                        <td>
                           <span id="msgapprov" style="font-size:xx-large; font-weight:bold"></span>
                        </td>
                        <%--Staff Type--%>
                        <td>
                            <asp:Label ID="lbl_staff_type" runat="server" Style="top: 10px; left: 6px;" Text="Staff Type"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_staff_type" runat="server" CssClass="txtcaps textbox1 txtheight2"
                                ReadOnly="true" BackColor="#ffffcc"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                            <%--ApproveDate & Time--%>
                            <td>
                                <asp:Label ID="lbl_staffappdatetime" runat="server" Style="top: 10px; left: 6px;" Text="Date & Time"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_staffappdate" runat="server" CssClass="txtcaps txtheight" BackColor="#ffffcc">
                                </asp:TextBox>
                                &nbsp;
                                <asp:TextBox ID="txt_staffapptime" runat="server" CssClass="txtcaps txtheight" BackColor="#ffffcc"></asp:TextBox>
                            </td>
                             <%--Purpose--%>
                        <td>
                            <asp:Label ID="lbl_staff_purpose" runat="server" Style="top: 10px; left: 6px;" Text="Purpose"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_staff_purpose" runat="server" CssClass="txtcaps textbox1 txtheight5" onkeyup="myFunCaps(this.id)"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="ftext_staff_purpose" runat="server" TargetControlID="txt_staff_purpose"
                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-&">
                            </asp:FilteredTextBoxExtender>
                        </td>
                        </tr>
                    <tr>
                        <%--Expected Date--%>
                        <td>
                            <asp:Label ID="lbl_staff_exp" runat="server" Style="top: 10px; left: 6px;" Text="Expected Date"></asp:Label>
                        </td>
                        <td>
                                    <asp:TextBox ID="txt_staff_exp" runat="server" CssClass="txtcaps textbox1 txtheight"
                                        OnTextChanged="txt_staffexpdate_TextChanged" BackColor="#ffffcc"></asp:TextBox>
                         &nbsp;
                        <%--Expected Time--%>
                            <asp:TextBox ID="txt_staff_exptime" runat="server" CssClass="txtcaps textbox1 txtheight" BackColor="#ffffcc"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                    <%--Entry Exit--%>
                        <td>
                            <asp:Label ID="lbl_staffexit" runat="server" Style="top: 10px; left: 6px;" Text="Entry/Exit By"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:RadioButton ID="rdo_staff_ins_trans" runat="server" RepeatDirection="Horizontal"
                                GroupName="stafftrans" Text="Institution Transport" onchange="return checkstaffrbowninst();"
                                OnCheckedChanged="rb_staffinst_CheckedChanged"></asp:RadioButton>
                            
                            <asp:RadioButton ID="rdo_staff_own_trans" runat="server" RepeatDirection="Horizontal"
                                GroupName="stafftrans" Text="Own Transport" onchange="return checkstaffrbowninst();"
                                OnCheckedChanged="rb_staffown_CheckedChanged"></asp:RadioButton>
                            <asp:RadioButton ID="rdo_staff_other_trans" runat="server" RepeatDirection="Horizontal"
                                GroupName="stafftrans" Text="Others" Checked="true" onchange="return checkstaffrbowninst();">
                            </asp:RadioButton>
                        </td>
                        </tr>
                        <tr>
                        <%--Entry Exit By Own Vehicle--%>
                        <td colspan="4">
                            <div id="divstaffownentryexit" runat="server" style="display: none;">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_staff_ownvehilno" runat="server" Style="top: 10px; left: 6px;"
                                                Text="Vehicle No"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_staff_ownvehilno" runat="server" CssClass="txtcaps txtheight2" MaxLength="15"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="ftext_staff_ownvehilno" runat="server" TargetControlID="txt_staff_ownvehilno"
                                FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" -">
                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_staff_owndrivername" runat="server" Style="top: 10px; left: 6px;"
                                                Text="Driver Name"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_staff_owndrivername" runat="server" CssClass="txtcaps txtheight2"></asp:TextBox>
                                             <asp:FilteredTextBoxExtender ID="ftext_staff_owndrivername" runat="server" TargetControlID="txt_staff_owndrivername"
                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    
                                        <td>
                                            <asp:Label ID="lbl_staff_ownmobno" runat="server" Style="top: 10px; left: 6px;" Text="Mobile No"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_staff_ownmobno" runat="server" CssClass="txtcaps txtheight2" MaxLength="10"></asp:TextBox>
                                             <asp:FilteredTextBoxExtender ID="ftext_staff_ownmobno" runat="server" TargetControlID="txt_staff_ownmobno"
                                FilterType="numbers" ValidChars="">
                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                    <%--Entry Exit By Institute Vehicle--%>
                        <td colspan="6">
                            <div id="divstaffinsentryexit" runat="server" style="display: none;">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_staff_vehilno" runat="server" Style="top: 10px; left: 6px;" Text="Vehicle No"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_staff_vehilno" runat="server" CssClass="txtcaps textbox1 txtheight"
                                                onblur="return getstaffdriverdet(this.value);" MaxLength="15"></asp:TextBox>
                                            <asp:AutoCompleteExtender ID="acext_staff_vehilno" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="getvehicle" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_staff_vehilno"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_staff_drvname" runat="server" Style="top: 10px; left: 6px;" Text="Driver Name"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_staff_drvname" runat="server" CssClass="txtcaps textbox1 txtheight2"
                                                ReadOnly="true" BackColor="#ffffcc"></asp:TextBox>
                                        </td>
                                   
                                        <td>
                                            <asp:Label ID="lbl_staff_route" runat="server" Style="top: 10px; left: 6px;" Text="Route"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_staff_route" runat="server" CssClass="txtcaps textbox1 txtheight"
                                                ReadOnly="true" BackColor="#ffffcc"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_staff_mob" runat="server" Style="top: 10px; left: 6px;" Text="Mobile No" MaxLength="10"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_staff_mob" runat="server" CssClass="txtcaps textbox1 txtheight"
                                                ReadOnly="true" BackColor="#ffffcc"></asp:TextBox>
                                        </td>
                                   
                                        <td>
                                            <asp:Label ID="lbl_staff_drop" runat="server" Style="top: 10px; left: 6px;" Text="Drop Stage"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_staff_drop" runat="server" CssClass="txtcaps textbox1 txtheight4"></asp:TextBox>
                                            <asp:AutoCompleteExtender ID="acext_staff_drop" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getstage" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_staff_drop"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                 </table>
               
                <br />
                <asp:Button ID="btn_staffok" runat="server" CssClass="textbox btn2" Text="Save" OnClientClick="return validstaff()" OnClick="btn_staffok_Click" />
                <asp:Button ID="btn_staffclear" runat="server" CssClass="textbox btn2" Text="Clear"
                    OnClientClick="return btnstaffclear();" /></div>
            </center>
            <%-- -----div_parent-------%>
            <center>
            <div id="div_parent" runat="server"  style="display: none; ">
                <center>
                   <table class="" width="98%">
                    <tr>
                    <%--in out--%>
                        <td align="right">
                            <div class="maindivstyle" align="center" style="border-radius: 7px; width: 150px;
                                height: 30px; font-size:larger;">
                                <asp:RadioButton ID="rb_parin" runat="server" RepeatDirection="Horizontal" GroupName="parinout"
                                    Text="In" Checked="true" onchange="return parentinoutclr();"></asp:RadioButton>
                                &nbsp;&nbsp;
                                <asp:RadioButton ID="rb_parout" runat="server" RepeatDirection="Horizontal" GroupName="parinout"
                                    Text="Out" onchange="return parentinoutclr();"></asp:RadioButton>
                            </div>
                        </td>
                        <%--admit not admit stud--%>
                         <td align="right">
                         <div class="maindivstyle" align="center" style="border-radius: 7px; width: 350px;height: 30px; font-size:larger;">
                                <asp:RadioButton ID="rb_adm_stud" runat="server" Text="Admitted Student" GroupName="studadmit"
                                    OnCheckedChanged="rb_adm_stud_CheckedChanged" Font-Size="Small" onchange="return checkstudadmit();"
                                    Checked="true" />
                           
                                <asp:RadioButton ID="rb_notadm_stud" runat="server" Font-Size="Small" Text="Not Admitted Student" GroupName="studadmit"
                                    OnCheckedChanged="rb_notadm_stud_CheckedChanged" onchange="return checkstudadmit();" />
                            </div>
                            </td>
                        <td align="right">
                            <div class="maindivstyle" align="center" style="border-radius: 7px; width: 300px;
                                height: 40px; font-size:large;">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_pardate" runat="server" Style="top: 10px; left: 6px;" Text="Date"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="upp_pardate" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_pardate" runat="server" CssClass="txtcaps textbox1 txtheight"
                                                        OnTextChanged="txt_satffdate_TextChanged" ReadOnly="true" AutoPostBack="true"></asp:TextBox>
                                                    <%--<asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_pardate" runat="server"
                                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                    </asp:CalendarExtender>--%>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_partime" runat="server" Style="top: 10px; left: 6px;" Text="Time"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_partime" runat="server" CssClass="textbox textbox1 txtheight"
                                                OnTextChanged="txt_stafftime_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                </center>
                <center>
                    <div id="div_adm_stud" runat="server" style="display: none;">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_stud_rollno" runat="server" Style="top: 10px; left: 6px;" Text="Roll No"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_stud_rollno" runat="server" CssClass="txtcaps txtheight2"
                                      onchange="return checkprno(this.value)"  onkeyup="return checkprno(this.value)" onblur="return pgetrno(this.value)" onfocus="return myFunction(this)"></asp:TextBox>
                                        <span style="color: Red;">*</span>
                                    <span id="prnomsg"></span>
                                    <asp:FilteredTextBoxExtender ID="ftext_stud_rollno" runat="server" TargetControlID="txt_stud_rollno"
                                        FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:AutoCompleteExtender ID="acext_stud_rollno" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="Getrnostud" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_stud_rollno"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_pdegree" runat="server" Style="top: 10px; left: 6px;" Text="Degree"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_pdegree" runat="server" CssClass="txtcaps txtheight"  BackColor="#ffffcc"></asp:TextBox>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_studname" runat="server" Style="top: 10px; left: 6px;" Text="Student Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_studname" runat="server" CssClass="txtcaps txtheight5" 
                                        onblur="return pgetname(this.value)"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="ftext_studname" runat="server" TargetControlID="txt_studname"
                                        FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-()&">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:AutoCompleteExtender ID="acext_studname" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_studname"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_dept1" runat="server" Style="top: 10px; left: 6px;" Text="Department"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_dept1" runat="server" CssClass="txtcaps txtheight4"  BackColor="#ffffcc"></asp:TextBox>
                                </td>
                                <td rowspan="3">
                                    <asp:Image ID="image5" runat="server" ImageUrl="" ToolTip="student's Photo" Style="height: 110px; width: 130px;" />
                                </td>
                                <td rowspan="3">
                                    <asp:Image ID="image6" runat="server" ImageUrl="" ToolTip="Parents Photo"  Style="height: 110px; width: 130px;" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_fname" runat="server" Style="top: 10px; left: 6px;" Text="Father Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_fname" runat="server" CssClass="txtcaps txtheight5" onblur="return getstuddetfrmfather(this.value)"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="ftext_fname" runat="server" TargetControlID="txt_fname"
                                        FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-()&">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:AutoCompleteExtender ID="acext_fname" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="GetFathername" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_fname"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_sem1" runat="server" Style="top: 10px; left: 6px;" Text="Semester"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_sem1" runat="server" CssClass="txtcaps" Width="50px"  BackColor="#ffffcc"></asp:TextBox>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_fmobile" runat="server" Style="top: 10px; left: 6px;" Text="Father Mobile No" MaxLength="10"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_fmobile" runat="server" CssClass="txtcaps txtheight5" onblur="return getdetfrmmob(this.value)"
                                        ></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="ftext_fmobile" runat="server" TargetControlID="txt_fmobile"
                                        FilterType="numbers,custom" ValidChars=" -,+">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:AutoCompleteExtender ID="acext_fmobile" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="GetFatherMobileNo" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_fmobile"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_section" runat="server" Style="top: 10px; left: 6px;" Text="Section"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_section" runat="server" CssClass="txtcaps" Width="50px"  BackColor="#ffffcc"></asp:TextBox>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_studrelation" runat="server" Style="top: 10px; left: 6px;" Text="Relationship of Student"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_studrelation" runat="server" CssClass="txtcaps txtheight5" onfocus="return myFunction(this)"></asp:TextBox>
                                    <span style="color: Red;">*</span>
                                    <asp:FilteredTextBoxExtender ID="ftext_studrelation" runat="server" TargetControlID="txt_studrelation"
                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                            </asp:FilteredTextBoxExtender>
                                </td>
                                 <td>
                                    <asp:Label ID="lbl_studtype1" runat="server" Style="top: 10px; left: 6px;" Text="Student Type"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_studtype1" runat="server" CssClass="txtcaps txtheight2"  BackColor="#ffffcc"></asp:TextBox>
                                </td>
                                <td colspan="2" rowspan="3">
                                    <asp:Image ID="image7" runat="server" ImageUrl="" ToolTip="Parents Photo"  Style="height: 110px; width: 130px;" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_purposevisit" runat="server" Style="top: 10px; left: 6px;" Text="Purpose of Visit"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_purposevisit" runat="server" CssClass="txtcaps txtheight5" onkeyup="myFunCaps(this.id)"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="ftext_purposevisit" runat="server" TargetControlID="txt_purposevisit"
                                        FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .,-&">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                
                               
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_meet" runat="server" Style="top: 10px; left: 6px;" Text="Meet"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:RadioButton ID="rb_meetstaff" runat="server" RepeatDirection="Horizontal" GroupName="studadmitmeet"
                                        Text="Staff" onchange="return checkstudadmitmeet();"></asp:RadioButton>
                                
                                    <asp:RadioButton ID="rb_meetoffice" runat="server" RepeatDirection="Horizontal" GroupName="studadmitmeet"
                                        Text="Office" OnCheckedChanged="rb_meetoffice_CheckedChanged" onchange="return checkstudadmitmeet();"
                                        Checked="true"></asp:RadioButton>
                               
                                    <asp:RadioButton ID="rb_meetothers" runat="server" RepeatDirection="Horizontal" GroupName="studadmitmeet"
                                        Text="Others" OnCheckedChanged="rb_meetothers_CheckedChanged" onchange="return checkstudadmitmeet();">
                                    </asp:RadioButton>
                                </td>
                                
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <div id="div_meetstaff" runat="server" style="display: none;">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_meetstaffid" runat="server" Style="top: 10px; left: 6px;" Text="Staff Code"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_meetstaffid" runat="server" CssClass="txtcaps txtheight" onblur="meetstaffdetbyid(this.value)"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="ftext_meetstaffid" runat="server" TargetControlID="txt_meetstaffid"
                                                        FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:AutoCompleteExtender ID="acext_meetstaffid" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="getstaffcode" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_meetstaffid"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="panelbackground">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                               
                                                <td>
                                                    <asp:Label ID="lbl_meetstaffname" runat="server" Style="top: 10px; left: 6px;" Text="Staff Name"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_meetstaffname" runat="server" CssClass="txtcaps txtheight5"
                                                        onblur="meetstaffdetbyname(this.value)"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="ftext_meetstaffname" runat="server" TargetControlID="txt_meetstaffname"
                                                        FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" .-&()">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:AutoCompleteExtender ID="acext_meetstaffname" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="getstaffnamewithdept" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_meetstaffname"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="panelbackground">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                                 <td>
                                                    <asp:Label ID="lbl_meetstaffdept" runat="server" Style="top: 10px; left: 6px;" Text="Department"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_meetstaffdept" runat="server" CssClass="txtcaps txtheight3"  BackColor="#ffffcc"></asp:TextBox>
                                                </td>
                                                
                                                <td>
                                                    <asp:Label ID="lbl_meetstaffdesg" runat="server" Style="top: 10px; left: 6px;" Text="Designation"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_meetstaffdesg" runat="server" CssClass="txtcaps txtheight2"  BackColor="#ffffcc"></asp:TextBox>
                                                </td>
                                        </table>
                                    </div>
                                    <div id="div_meetoffice" runat="server" style="display: none;">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_staffname1" runat="server" Style="top: 10px; left: 6px;" Text="Staff Name"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_staffname1" runat="server"  CssClass="txtcaps txtheight5"
                                                        onblur="return getmeetoffstaffname(this.value)"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="ftext_staffname1" runat="server" TargetControlID="txt_staffname1"
                                                        FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-&()">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:AutoCompleteExtender ID="acext_staffname1" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="getstaffnamewithdept" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_staffname1"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="panelbackground">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                          
                                                <td>
                                                    <asp:Label ID="lbl_dpt" runat="server" Style="top: 10px; left: 6px;" Text="Department"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_dpt" runat="server" Width="200px" CssClass="txtcaps txtheight3"  BackColor="#ffffcc"></asp:TextBox>
                                                </td>
                                           
                                                <td>
                                                    <asp:Label ID="lbl_desgtn" runat="server" Style="top: 10px; left: 6px;" Text="Designation"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_desgtn" runat="server" CssClass="txtcaps txtheight2" BackColor="#ffffcc"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="div_meetothers" runat="server" style="display: none;">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_name1" runat="server" Style="top: 10px; left: 6px;" Text="Name"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_name1" runat="server" CssClass="txtcaps txtheight4"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="ftext_name1" runat="server" TargetControlID="txt_name1"
                                                        FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-&">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                            
                                                <td>
                                                    <asp:Label ID="lbl_relation" runat="server" Style="top: 10px; left: 6px;" Text="Relationship"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_relation" runat="server" CssClass="txtcaps txtheight5"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="ftext_relation" runat="server" TargetControlID="txt_relation"
                                                        FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-&">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                           
                                                <td>
                                                    <asp:Label ID="lbl_moblno" runat="server" Style="top: 10px; left: 6px;" Text="Mobile No" MaxLength="10"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_moblno" runat="server" CssClass="txtcaps txtheight2"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="ftext_moblno" runat="server" TargetControlID="txt_moblno"
                                                        FilterType="numbers" ValidChars="">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="div_notadm_stud" runat="server" style="display: none;">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_name2" runat="server" Style="top: 10px; left: 6px;" Text="Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_name2" runat="server" CssClass="txtcaps txtheight5" onfocus="return myFunction(this)"></asp:TextBox>
                                    <span style="color: Red;">*</span>
                                      <asp:FilteredTextBoxExtender ID="ftext_name2" runat="server" TargetControlID="txt_name2"
                                                        FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-&">
                                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_mob" runat="server" Style="top: 10px; left: 6px;" Text="Mobile No"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_mob" runat="server" CssClass="txtcaps txtheight2" onfocus="return myFunction(this)"  MaxLength="10"></asp:TextBox>
                                    <span style="color: Red;">*</span>
                                      <asp:FilteredTextBoxExtender ID="ftext_mob" runat="server" TargetControlID="txt_mob"
                                                        FilterType="numbers" ValidChars="">
                                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_addrs" runat="server" Style="top: 10px; left: 6px;" Text="Address"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_addrs" runat="server" CssClass="txtcaps txtheight4"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="ftext_addrs" runat="server" TargetControlID="txt_addrs"
                                                        FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" .,/-&">
                                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_visit" runat="server" Style="top: 10px; left: 6px;" Text="Purpose of Visit"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_visit" runat="server" CssClass="txtcaps txtheight5" onkeyup="myFunCaps(this.id)"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="ftext_visit" runat="server" TargetControlID="txt_visit"
                                        FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .,-&">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_city" runat="server" Style="top: 10px; left: 6px;" Text="City"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_city" runat="server" CssClass="txtcaps txtheight4"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="ftext_city" runat="server" TargetControlID="txt_city"
                                                        FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                                    </asp:FilteredTextBoxExtender>
                                </td>
                               
                                <td>
                                </td>
                                <td colspan="2" rowspan="3">
                                    <asp:Image ID="image8" runat="server" ImageUrl="" ToolTip="Applicant's Photo" Style="height: 110px; width: 130px;" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_district" runat="server" Style="top: 10px; left: 6px;" Text="District"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_district" runat="server" CssClass="txtcaps txtheight4"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="ftext_district" runat="server" TargetControlID="txt_district"
                                                        FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                 <td>
                                    <asp:Label ID="lbl_state" runat="server" Style="top: 10px; left: 6px;" Text="State"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_state" runat="server" CssClass="txtcaps txtheight4"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="ftext_state" runat="server" TargetControlID="txt_state"
                                                        FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_pincode" runat="server" Style="top: 10px; left: 6px;" Text="Pincode"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_pincode" runat="server" CssClass="txtcaps txtheight" MaxLength="8"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="ftext_pincode" runat="server" TargetControlID="txt_pincode"
                                                        FilterType="numbers" ValidChars="">
                                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </div>
                </center>
                <asp:Button ID="btn_parentok" runat="server" CssClass="textbox btn2" Text="Save" OnClientClick="return validparent();" OnClick="btn_parentok_Click" />
                <asp:Button ID="btn_parentclear" runat="server" CssClass="textbox btn2" Text="Clear"
                    OnClientClick="return btnparentclear();" />
            </div>
            </center>
            <%-- ----end of div_parent------%>
                 
              <center>
            <div id="div_visitor" runat="server"  style="display: none; z-index:-100;">
            <table class="" width="98%">
                    <tr>
                        <%--in out--%>
                        <td align="right">
                            <div class="maindivstyle" align="center" style="border-radius: 7px; width: 150px;
                                height: 30px; font-size:larger;">
                                <asp:RadioButton ID="rb_visitin" runat="server" RepeatDirection="Horizontal" GroupName="visitinout"
                                    Text="In" Checked="true" onchange="return visitorinoutclr();"></asp:RadioButton>
                                &nbsp;&nbsp;
                                <asp:RadioButton ID="rb_visitout" runat="server" RepeatDirection="Horizontal" GroupName="visitinout"
                                    Text="Out" onchange="return visitorinoutclr();"></asp:RadioButton>
                            </div>
                        </td>
                        <%--admit not admit stud--%>
                        <td align="right">
                         <div id="mass" class="maindivstyle" runat="server" visible="false" align="center" style="border-radius: 7px; width: 350px;height: 30px; font-size:larger;" >
                               <asp:RadioButton ID="rb_company" runat="server" RepeatDirection="Horizontal" GroupName="visitor"
                                Text="Company" OnCheckedChanged="rb_company_CheckedChanged" Checked="true" onchange="return rb_indcompname();"></asp:RadioButton>
                        
                            <asp:RadioButton ID="rb_individual" runat="server" RepeatDirection="Horizontal" GroupName="visitor"
                                Text="Individual" OnCheckedChanged="rb_individual_CheckedChanged" onchange="return rb_indcompname();"></asp:RadioButton>
                  
                         </div>
                         </td>
                        <%--Date & Time--%>
                        <td align="right">
                            <div class="maindivstyle" align="center" style="border-radius: 7px; width: 300px;
                                height: 40px; font-size:large;">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_visitdate" runat="server" Style="top: 10px; left: 6px;" Text="Date"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="upp_visitdate" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_visitdate" runat="server" CssClass="txtcaps textbox1 txtheight"
                                                         OnTextChanged="txt_satffdate_TextChanged" ReadOnly="true"></asp:TextBox>
                                                    <%--<asp:CalendarExtender ID="CalendarExtender5" TargetControlID="txt_visitdate" runat="server"
                                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                    </asp:CalendarExtender>--%>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_visittime" runat="server" Style="top: 10px; left: 6px;" Text="Time"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_visittime" runat="server" CssClass="txtcaps textbox1 txtheight"
                                                OnTextChanged="txt_stafftime_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                   </tr>
               </table>
               <%--barath--%>
               <br />
               <table class="maindivstyle">
               <tr>
                    <%--With & Without Appointment--%>
                        <td colspan="3">
                            <asp:RadioButton ID="rb_withap" runat="server" RepeatDirection="Horizontal" GroupName="visitorappt"
                                Text="With Appointment" OnCheckedChanged="rb_withap_CheckedChanged" onchange="return visitorappoint();"
                               ></asp:RadioButton>
                       
                            <asp:RadioButton ID="rb_withoutap" runat="server" RepeatDirection="Horizontal" GroupName="visitorappt"
                                Text="Without Appointment" Checked="true" OnCheckedChanged="rb_withoutap_CheckedChanged" onchange="return visitorappoint();">
                            </asp:RadioButton>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <div id="div_withappoint" runat="server" style="display: none;">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_sname" runat="server" Style="top: 10px; left: 6px;" Text="Staff Name"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="sname" runat="server" CssClass="txtcaps txtheight5" onblur="return visitorstaffdet(this.value)"></asp:TextBox>
                                            
                                            <asp:AutoCompleteExtender ID="acext_visitstaffname" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="getstaffnamewithdept" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="sname"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                         <td>
                                            <asp:Label ID="lbl_dpt1" runat="server" Style="top: 10px; left: 6px;" Text="Department"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_dpt1" runat="server" BackColor="#ffffcc" CssClass="txtcaps txtheight4"></asp:TextBox>
                                        </td>
                                        <td></td>
                                        <td colspan="2" rowspan="2" align="right">
                                            <asp:Image ID="image10" runat="server" ImageUrl="" Style="height: 90px; width: 130px;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">
                                            <asp:Label ID="lbl_desg1" runat="server" Style="top: 10px; left: 6px;" Text="Designation"></asp:Label>
                                       
                                            <asp:TextBox ID="txt_desg1" runat="server" BackColor="#ffffcc" CssClass="txtcaps txtheight3"></asp:TextBox>
                                        
                                            <asp:Label ID="lbl_type" runat="server" Style="top: 10px; left: 6px;" Text="Type"></asp:Label>
                                       
                                            <asp:TextBox ID="txt_type" runat="server" BackColor="#ffffcc" CssClass="txtcaps txtheight2"></asp:TextBox>
                                        
                                            <asp:Label ID="lbl_mblno" runat="server" Style="top: 10px; left: 6px;" Text="Mobile No"></asp:Label>
                                       
                                            <asp:TextBox ID="mblno" runat="server" BackColor="#ffffcc" CssClass="txtcaps txtheight2" MaxLength="10"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>                            
                        </td>
                    </tr>
                   
               </table>
               <br />
                <table width="98%">
                <tr>
                <td colspan="2">
                                <span id="statusmsgvis" style="font-size:x-large ;font-weight:bold ;" ></span>
                            </td>
                </tr>
                 <tr>
                    <td colspan="5">
                    <div id="imgdiv2" runat="server" visible="false" style="height: 100%; z-index: 100px; 
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; margin-top:50px; 
            left: 0%;">
            <center>
                    <div id="panel_erroralert" runat="server" class="table" style="background-color: White; margin-top: -17px;margin-left: 347px;
                        height: 100px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                         border-radius: 10px;">
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_erroralert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btn_erroralert" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                width: 65px;" Text="Ok" runat="server" OnClick="btnerrorclose_Click" OnClientClick="btnerrorclose()" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                    </div>
                    </center>
            </div>
            </td>
                    </tr>
                <tr>

                 <td>
                            <asp:Label ID="Lblgatepass" runat="server" Style="top: 10px; left: 6px;" Text="Gatepass No "></asp:Label>
                        </td>
                        <td><%--<asp:TextBox ID="TextBox1" runat="server" Text=""   CssClass="txtcaps  txtheight5"   OnTextChanged="TextBox1_Changed" onblur="return getapplNo(this.value)"  
                              onfocus="return myFunction(this)"></asp:TextBox> --%>

                              <asp:TextBox ID="TextBox1" runat="server" Text="" AutoPostBack="true"  CssClass="txtcaps  txtheight5"   OnTextChanged="TextBox1_Changed"  onblur="return visitorcompdet(this.value)"  onkeypress="return enterscript(event)"  Enabled="false"></asp:TextBox>

                              
                             <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="getgateno" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="TextBox1"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="panelbackground">
                                </asp:AutoCompleteExtender> 
                                  <asp:Button ID="Button1" runat="server" CssClass="textbox btn2" Text="Details"  OnClick="btn_visitorok1_Click" BackColor= "aqua"   /><%--OnClientClick="return visitorcompdet(this.value)"--%>
                                
                                </td><%--onblur="bindvisitorname(this.value)"--%>
                               
                </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_compname" runat="server" Style="top: 10px; left: 6px;" Text="Company Name"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_compname" runat="server" CssClass="txtcaps txtheight5"         OnTextChanged="txt_compname_Changed"  onfocus="return myFunction(this)"></asp:TextBox>
                            <span id="cnamespan" style="color: Red;">*</span>
                            <span id="cnamemsg"></span>
                          
                            <asp:AutoCompleteExtender ID="acext_compname" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="getcompname" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_compname"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="panelbackground">
                            </asp:AutoCompleteExtender>
                        </td>
                        <%--<td>
                            <asp:Label ID="Lbl" runat="server" Style="top: 10px; left: 6px;" Text="Gate No"></asp:Label>
                        </td>
                        <td>
                     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                        <asp:DropDownList ID="ddl_gateno" runat="server" CssClass="textbox1  ddlheight1" Visible="True"
                             AutoPostBack="True">
                        </asp:DropDownList>
                         </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>--%>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_name4" runat="server" Style="top: 10px; left: 6px;" Text="Name"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_name4" runat="server" CssClass="txtcaps txtheight5"  onfocus="return myFunction(this)" ></asp:TextBox>
                            <span style="color: Red;">*</span>
                           
                              <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="getcomppername" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_name4"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="panelbackground">
                            </asp:AutoCompleteExtender>
                        </td>
                         <td>
                            <asp:Label ID="lbl_mno" runat="server" Style="top: 10px; left: 6px;" Text="Mobile No" MaxLength="10"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_mno" runat="server" CssClass="txtcaps txtheight2"></asp:TextBox><%--onkeyup="return visitorcompdetmobileno(this.value)" onblur="return visitorcompdet(this.value)"--%>
                            <asp:FilteredTextBoxExtender ID="ftext_mno" runat="server" TargetControlID="txt_mno"
                                FilterType="numbers" ValidChars="">
                            </asp:FilteredTextBoxExtender>  <span style="color: Red;">*</span>
                          
                        </td>
                        <td colspan="2" rowspan="3" align="center">
                            <asp:Image ID="image9" runat="server" ImageUrl="" ToolTip="Company/Employee Photo" Style="height: 90px; width: 130px;" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_desgn" runat="server" Style="top: 10px; left: 6px;" Text="Designation"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_desgn" runat="server" CssClass="txtcaps txtheight4"></asp:TextBox>
                            
                        </td>
                       
                       <td>
                            <asp:Label ID="lbl_str" runat="server" Style="top: 10px; left: 6px;" Text="Address"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_str" runat="server" CssClass="txtcaps txtheight5" ></asp:TextBox><%--onblur="return visitorcompdet(this.value)"--%>
                            
                        </td>
                        
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_dep" runat="server" Style="top: 10px; left: 6px;" Text="Department"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_dep" runat="server" CssClass="txtcaps txtheight4"></asp:TextBox>
                            
                        </td>
                       
                          <td>
                            <asp:Label ID="lbl_cty" runat="server" Style="top: 10px; left: 6px;" Text="City"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_cty" runat="server" CssClass="txtcaps txtheight4"></asp:TextBox>
                             
                        </td>
                    </tr>
                    <tr>
                         <td>
                            <asp:Label ID="lbl_visit1" runat="server" Style="top: 10px; left: 6px;" Text="Purpose of Visit"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_visit1" runat="server" CssClass="txtcaps txtheight5" onkeyup="myFunCaps(this.id)"></asp:TextBox>
                             
                        </td>
                      
                             <td>
                            <asp:Label ID="lbl_dis" runat="server" Style="top: 10px; left: 6px;" Text="District"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_dis" runat="server" CssClass="txtcaps txtheight4"></asp:TextBox>
                            
                        </td>
                    </tr>
                    <tr>
                     <td align="center">
                    
                            <asp:RadioButton ID="rb_ret" runat="server" RepeatDirection="Horizontal" GroupName="s1"
                                Text="Return" onchange="return visitorreturn();" OnCheckedChanged="rb_ret_CheckedChanged" Checked="true" visible="false"></asp:RadioButton>
                        </td>
                        <td>
                            <asp:RadioButton ID="rb_notret" runat="server" RepeatDirection="Horizontal" GroupName="s1"
                                Text="Not Return" onchange="return visitorreturn();" OnCheckedChanged="rb_notret_CheckedChanged" visible="false"></asp:RadioButton>
                        </td>
                   
                       
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_exprettime1" runat="server" Style="top: 10px; left: 6px;" Text="Expected Return Time" visible="false"></asp:Label>
                        </td>
                        <td>
                        <asp:DropDownList ID="ddl_hrs" runat="server" CssClass="txtcaps" height="25px" Width="50px" visible="false" ></asp:DropDownList>
                        <asp:DropDownList ID="ddl_mins" runat="server" CssClass="txtcaps" height="25px" Width="50px" visible="false"></asp:DropDownList>
                        <asp:DropDownList ID="ddl_ampm" runat="server" CssClass="txtcaps" height="25px" Width="50px" visible="false"> </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbl_stat" runat="server" Style="top: 10px; left: 6px;" Text="State"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_stat" runat="server" CssClass="txtcaps txtheight4"></asp:TextBox>
                             
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_vehic" runat="server" Style="top: 10px; left: 6px;" Text="Vehicle"></asp:Label>
                        </td>
                        <td>
                            <asp:RadioButton ID="rb_vehyes" runat="server" RepeatDirection="Horizontal" GroupName="s2"
                                Text="Yes" OnCheckedChanged="rb_vehyes_CheckedChanged" onchange="return visitorvehicle();">
                            </asp:RadioButton>
                            <asp:RadioButton ID="rb_vehno" runat="server" RepeatDirection="Horizontal" GroupName="s2"
                                Text="No" OnCheckedChanged="rb_vehno_CheckedChanged" Checked="true" onchange="return visitorvehicle();">
                            </asp:RadioButton>
                        </td>
                    </tr>
                    <tr>
                    <td></td>
                    <%--Vehicle Yes--%>
                        <td colspan="3">
                            <div id="div_vehyes" runat="server">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_vehtype" runat="server" Style="top: 10px; left: 6px;" Text="Type of Vehicle"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_vehtype" runat="server" CssClass="txtcaps txtheight2"></asp:TextBox>
                                             
                                        </td>
                                    
                                        <td>
                                            <asp:Label ID="lbl_vehno1" runat="server" Style="top: 10px; left: 6px;" Text="Vehicle No"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_vehno1" runat="server" CssClass="txtcaps txtheight2" style="text-transform:uppercase;" MaxLength="15"></asp:TextBox>
                                             
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr><td colspan="4">
                  <center>
                                <div id="div_withoutappoint" runat="server" style="display: none;">
                                    <asp:RadioButton ID="rb_staff1" runat="server" RepeatDirection="Horizontal" GroupName="rbvisitormeet"
                                        Text="Staff" Checked="true"  onchange="return rbvisitormeet();"></asp:RadioButton>
                                    <asp:RadioButton ID="rb_office1" runat="server" RepeatDirection="Horizontal" GroupName="rbvisitormeet"
                                        Text="Office" onchange="return rbvisitormeet();"></asp:RadioButton>
                                    <asp:RadioButton ID="rb_others1" runat="server" RepeatDirection="Horizontal" GroupName="rbvisitormeet"
                                        Text="Others" onchange="return rbvisitormeet();"></asp:RadioButton>
                                </div>
                            </center>
                          </td>  </tr>
                    <tr>
                        <td colspan="6">
                            <div id="div_visitormeetstaff" runat="server" style="display: none;">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_visitormeetstaffid" runat="server" Style="top: 10px; left: 6px;"
                                                Text="Staff Code"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_visitormeetstaffid" runat="server" CssClass="txtcaps txtheight"
                                                onblur="visitormeetstaffdetbyid(this.value)"></asp:TextBox>
                                           
                                            <asp:AutoCompleteExtender ID="acext_visitormeetstaffid" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="getstaffcode" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_visitormeetstaffid"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_visitormeetstaffname" runat="server" Style="top: 10px; left: 6px;"
                                                Text="Staff Name"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_visitormeetstaffname" runat="server" CssClass="txtcaps txtheight4"
                                                onblur="visitormeetstaffdetbyname(this.value)"></asp:TextBox>
                                           
                                            <asp:AutoCompleteExtender ID="acext_visitormeetstaffname" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="getstaffnamewithdept" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_visitormeetstaffname"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_visitormeetstaffdept" runat="server" Style="top: 10px; left: 6px;"
                                                Text="Department"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_visitormeetstaffdept" runat="server" BackColor="#ffffcc" CssClass="txtcaps txtheight3"></asp:TextBox>
                                        </td>
                                        
                                        <td>
                                            <asp:Label ID="lbl_visitormeetstaffdesg" runat="server" Style="top: 10px; left: 6px;"
                                                Text="Designation"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_visitormeetstaffdesg" runat="server" BackColor="#ffffcc" CssClass="txtcaps txtheight2"></asp:TextBox>
                                        </td>
                                </table>
                            </div>
                            <div id="div_visitormeetoffice" runat="server" style="display: none;">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_visitormeetoffname" runat="server" Style="top: 10px; left: 6px;"
                                                Text="Staff Name"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_visitormeetoffname" runat="server" CssClass="textbox txtheight5"
                                                onblur="return visitormeetofficedetbyname(this.value)"></asp:TextBox>
                                            
                                            <asp:AutoCompleteExtender ID="acext_visitormeetoffname" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="getstaffnamewithdept" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_visitormeetoffname"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                   
                                        <td>
                                            <asp:Label ID="lbl_visitormeetoffdept" runat="server" Style="top: 10px; left: 6px;"
                                                Text="Department"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_visitormeetoffdept" runat="server" Width="200px" BackColor="#ffffcc" CssClass="txtcaps txtheight4"></asp:TextBox>
                                        </td>
                                   
                                        <td>
                                            <asp:Label ID="lbl_visitormeetoffdesg" runat="server" Style="top: 10px; left: 6px;"
                                                Text="Designation"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_visitormeetoffdesg" runat="server" BackColor="#ffffcc" CssClass="txtcaps txtheight3"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="div_visitormeetother" runat="server" style="display: none;">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_visitormeetothername" runat="server" Style="top: 10px; left: 6px;"
                                                Text="Name"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_visitormeetothername" runat="server" CssClass="txtcaps txtheight5"></asp:TextBox>
                                            
                                        </td>
                                    
                                        <td>
                                            <asp:Label ID="lbl_visitormeetotherrel" runat="server" Style="top: 10px; left: 6px;"
                                                Text="Relationship"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_visitormeetotherrel" runat="server" CssClass="txtcaps txtheight4"></asp:TextBox>
                                             
                                        </td>
                                   
                                        <td>
                                            <asp:Label ID="lbl_visitormeetothermob" runat="server" Style="top: 10px; left: 6px;"
                                                Text="Mobile No"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_visitormeetothermob" runat="server" CssClass="txtcaps txtheight2" MaxLength="10"></asp:TextBox>
                                           
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                <asp:Button ID="btn_visitorok" runat="server" CssClass="textbox btn2" Text="Save" OnClientClick="return validvisitor()" OnClick="btn_visitorok_Click" />
                <asp:Button ID="btn_visitorclear" runat="server" CssClass="textbox btn2" Text="Clear"
                   OnClientClick="return btncompanyclear();" />
            </div>
            </center>
            <%-- ----------end of div_visitor------ --%>
            <center>
            <div id="div_material" runat="server"  style="display: none;">
              <table class="" width="98%">
                    <tr>
                        <%--in out--%>
                        <td align="right">
                            <div class="maindivstyle" align="center" style="border-radius: 7px; width: 150px;
                                height: 30px; font-size:larger;">
                                <asp:RadioButton ID="rb_matin" runat="server" RepeatDirection="Horizontal" GroupName="matinout"
                                    Text="In" Checked="true" onchange="return materialinoutclr();"></asp:RadioButton>
                                &nbsp;&nbsp;
                                <asp:RadioButton ID="rb_matout" runat="server" RepeatDirection="Horizontal" GroupName="matinout"
                                    Text="Out" onchange="return materialinoutclr();"></asp:RadioButton>
                            </div>
                        </td>
                        <%--Material Type--%>
                        <td align="right">
                         <div class="maindivstyle" align="center" style="border-radius: 7px; width: 400px;
                                height: 30px; font-size:larger;">
                                <asp:RadioButton ID="rb_ordmaterial" runat="server" RepeatDirection="Horizontal"
                                    GroupName="material" Text="Ordered Material" onchange="return rbmaterial();"
                                    Checked="true"></asp:RadioButton>
                           
                                <asp:RadioButton ID="rb_other" runat="server" RepeatDirection="Horizontal" GroupName="material"
                                    Text="Others" onchange="return rbmaterial();"></asp:RadioButton>
                           
                                <asp:RadioButton ID="rb_service" runat="server" RepeatDirection="Horizontal" GroupName="material"
                                    Text="Service" onchange="return rbmaterial();"></asp:RadioButton>
                         </div>
                         </td>
                        <%--Date & Time--%>
                        <td align="right">
                            <div class="maindivstyle" align="center" style="border-radius: 7px; width: 300px;
                                height: 40px; font-size:large;">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_materialdate" runat="server" Style="top: 10px; left: 6px;" Text="Date"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="upp_materialdate" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_materialdate" runat="server" CssClass="txtcaps textbox1 txtheight"
                                                        OnTextChanged="txt_satffdate_TextChanged" ReadOnly="true" AutoPostBack="true"></asp:TextBox>
                                                    <%--<asp:CalendarExtender ID="CalendarExtender6" TargetControlID="txt_materialdate" runat="server"
                                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                    </asp:CalendarExtender>--%>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_materialtime" runat="server" Style="top: 10px; left: 6px;" Text="Time"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_materialtime" runat="server" CssClass="txtcaps textbox1 txtheight"
                                                OnTextChanged="txt_stafftime_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                   </tr>
               </table>
               
                <div id="div_ordermaterial" runat="server" style="display: none;">
                    <center>
                        <table width="98%">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_purordno" runat="server" Style="top: 10px; left: 6px;" Text="Purchase Order No"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_purordno" runat="server" CssClass="txtcaps txtheight2" onchange="return checkpono(this.value)" onkeyup="return checkpono(this.value)"
                                        onblur="return getmatpurdet(this.value)" onfocus="return myFunction(this)"></asp:TextBox>
                                        <span style="color: Red;">*</span>
                                    <span id="ponomsg"></span>
                                    <asp:FilteredTextBoxExtender ID="ftext_purordno" runat="server" TargetControlID="txt_purordno"
                                        FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:AutoCompleteExtender ID="acext_purordno" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="getorderno" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_purordno"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_addr" runat="server" Style="top: 10px; left: 6px;" Text="Address"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_addr" runat="server" CssClass="txtcaps txtheight5" BackColor="#ffffcc"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_suppliername" runat="server" Style="top: 10px; left: 6px;" Text="Supplier Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_suppliername" runat="server" CssClass="txtcaps txtheight5" BackColor="#ffffcc"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_city1" runat="server" Style="top: 10px; left: 6px;" Text="City"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txt_city1" runat="server" CssClass="txtcaps txtheight3" BackColor="#ffffcc"></asp:TextBox>
                                
                                    <asp:Label ID="lbl_dist" runat="server" Style="top: 10px; left: 6px;" Text="District"></asp:Label>
                                
                                 <asp:TextBox ID="txt_dist" runat="server" CssClass="txtcaps txtheight3" BackColor="#ffffcc"></asp:TextBox>
                                 </td>
                            </tr>
                            <tr>
                                <td colspan="2" rowspan="4">
                                <div id="div_matitem" runat="server" style=" display:none; top:4%; border-color:Gray; border-width: 3px; border-style: solid; 
                    background-color: White; position: relative; width: 380px;">
                    <table class="tableStyle" width="380px">
                        <tbody>
                            <tr class="tableHeader">
                             
                                <td style="width: 5px;">
                                    S.No
                                </td>
                                 <td style="width: 5px;">
                                  Select
                                </td>
                                <td style="width: 80px;">
                                    Item Code
                                </td>
                                <td style="width: 80px;">
                                    Name
                                </td>
                                <td style="width: 50px;">
                                    App.Qty
                                </td>
                            </tr>
                        </tbody>
                    </table>
                     <div id="mydiv" style="height: 100px; width: 380px; overflow: scroll; overflow-x: hidden;
                        overflow-y: scroll;">
                    </div>
                </div>
                                </td>
                            </tr>
                            
                            <tr>
                                 <td>
                                    <asp:Label ID="lbl_state1" runat="server" Style="top: 10px; left: 6px;" Text="State"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txt_state1" runat="server" CssClass="txtcaps txtheight3" BackColor="#ffffcc"></asp:TextBox>
                                
                                    <asp:Label ID="lbl_pincode1" runat="server" Style="top: 10px; left: 6px;" Text="Pincode"></asp:Label>
                                
                                    <asp:TextBox ID="txt_pincode1" runat="server" CssClass="txtcaps txtheight" BackColor="#ffffcc"></asp:TextBox>
                                </td>
                            </tr>
                            
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_contperson" runat="server" Style="top: 10px; left: 6px;" Text="Contact person"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_contperson" runat="server" CssClass="txtcaps txtheight3" BackColor="#ffffcc"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_name3" runat="server" Style="top: 10px; left: 6px;" Text="Name"></asp:Label>
                              </td>
                               <td colspan="3">
                                    <asp:TextBox ID="txt_name3" runat="server" CssClass="txtcaps txtheight3" BackColor="#ffffcc"></asp:TextBox>
                                     <asp:Label ID="lbl_mobileno" runat="server" Style="top: 10px; left: 6px;" Text="Mobile No" MaxLength="10"></asp:Label>
                               
                                    <asp:TextBox ID="txt_mobileno" runat="server" CssClass="txtcaps txtheight2" BackColor="#ffffcc"></asp:TextBox>
                               
                                </td>
                            </tr>
                           
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_entryby1" runat="server" Style="top: 10px; left: 6px;" Text="Entry/Exit By"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:RadioButton ID="rb_materialinsveh" runat="server" RepeatDirection="Horizontal"
                                        GroupName="materialvehicle" Text="Institution Vehicle" onchange="return materialentryby();"
                                        Checked="true"></asp:RadioButton>
                               &nbsp;&nbsp;
                                    <asp:RadioButton ID="rb_materialotherveh" runat="server" RepeatDirection="Horizontal"
                                        GroupName="materialvehicle" Text="Others" onchange="return materialentryby();">
                                    </asp:RadioButton>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                               
                                <td colspan="4">
                                    <div id="div_metr_entryby" runat="server">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_vehno" runat="server" Style="top: 10px; left: 6px;" Text="Vehicle No"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_vehno" runat="server" CssClass="txtcaps txtheight1" onblur="return getmatentbyinsvehdet(this.value)" MaxLength="15"></asp:TextBox>
                                                     <asp:FilteredTextBoxExtender ID="ftext_vehno" runat="server" TargetControlID="txt_vehno"
                                                    FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" .-&">
                                                </asp:FilteredTextBoxExtender>
                                                    <asp:AutoCompleteExtender ID="acext_vehno" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="getvehicle" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_vehno"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="panelbackground">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_drivname" runat="server" Style="top: 10px; left: 6px;" Text="Driver Name"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_drivname" runat="server" CssClass="txtcaps txtheight3" BackColor="#ffffcc"></asp:TextBox>
                                                </td>
                                            
                                                <td>
                                                    <asp:Label ID="lbl_vehitype" runat="server" Style="top: 10px; left: 6px;" Text="Vehicle type"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_vehitype" runat="server" CssClass="txtcaps txtheight1" BackColor="#ffffcc"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_mobno" runat="server" Style="top: 10px; left: 6px;" Text="Mobile No" MaxLength="10"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_mobno" runat="server" CssClass="txtcaps txtheight1" BackColor="#ffffcc"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="div_metr_others" runat="server">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_vehino" runat="server" Style="top: 10px; left: 6px;" Text="Vehicle No"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_vehino" runat="server" CssClass="txtcaps txtheight1" MaxLength="15"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="ftext_vehino" runat="server" TargetControlID="txt_vehino"
                                                        FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" -">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_bbyname" runat="server" Style="top: 10px; left: 6px;" Text="Brought by Name"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_bbyname" runat="server" CssClass="txtcaps txtheight3"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="ftext_bbyname" runat="server" TargetControlID="txt_bbyname"
                                                        FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-&">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                           
                                                <td>
                                                    <asp:Label ID="lbl_vehitype1" runat="server" Style="top: 10px; left: 6px;" Text="Vehicle type"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_vehitype1" runat="server" CssClass="txtcaps txtheight1"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="ftext_vehitype1" runat="server" TargetControlID="txt_vehitype1"
                                                        FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-&">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_mobno1" runat="server" Style="top: 10px; left: 6px;" Text="Mobile No" MaxLength="10"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_mobno1" runat="server" CssClass="txtcaps txtheight1"></asp:TextBox>
                                                     <asp:FilteredTextBoxExtender ID="ftext_mobno1" runat="server" TargetControlID="txt_mobno1"
                                                        FilterType="numbers" ValidChars="">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
                <div id="div_material_others" runat="server" style="display: none;">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Llbl_itemname" runat="server" Style="top: 10px; left: 6px;" Text="Item Name"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_itemname" runat="server" CssClass="txtcaps txtheight5" onblur="return getitemdet(this.value)" onfocus="return myFunction(this)"></asp:TextBox>
                                <span style="color: Red;">*</span>
                                <asp:AutoCompleteExtender ID="acext_itemname" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="getitemname" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_itemname"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="panelbackground">
                                </asp:AutoCompleteExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_qty" runat="server" Style="top: 10px; left: 6px;" Text="Quantity"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_qty" runat="server" CssClass="txtcaps txtheight1" onfocus="return myFunction(this)"></asp:TextBox>
                                <span style="color: Red;">*</span>
                                <asp:FilteredTextBoxExtender ID="ftext_qty" runat="server" TargetControlID="txt_qty"
                                                        FilterType="numbers" ValidChars=".">
                                                    </asp:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_measure" runat="server" Style="top: 10px; left: 6px;" Text="Measure"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_measure" runat="server" CssClass="txtcaps txtheight1" BackColor="#ffffcc"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <asp:Button ID="btn_add" runat="server" CssClass="txtcaps btn2" Text="Add" OnClick="btn_add_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:Button ID="btn_materialok" runat="server" CssClass="textbox btn2" Text="Save" OnClientClick="return validmaterial()"
                    OnClick="btn_materialok_Click" />
                <asp:Button ID="btn_materialclear" runat="server" CssClass="textbox btn2" Text="Clear"
                    OnClientClick="return btnmaterialclear();" />
            </div>
            </center>
            <%-- -------end of div_material--------%>
            <center>
            <div id="div_vehicle" runat="server"  style="display: none; ">
            <table class="" width="98%">
                    <tr>
                        <%--in out--%>
                        <td align="right">
                            <div class="maindivstyle" align="center" style="border-radius: 7px; width: 150px;
                                height: 30px; font-size:larger;">
                                <asp:RadioButton ID="rb_vehiclein" runat="server" RepeatDirection="Horizontal" GroupName="vehicleinout"
                                    Text="In" Checked="true" onchange="return vehicleinoutclr();"></asp:RadioButton>
                                &nbsp;&nbsp;
                                <asp:RadioButton ID="rb_vehicleout" runat="server" RepeatDirection="Horizontal" GroupName="vehicleinout"
                                    Text="Out"  onchange="return vehicleinoutclr();"></asp:RadioButton>
                            </div>
                        </td>
                        <%--Vehicle Type--%>
                        <td align="right">
                         <div class="maindivstyle" align="center" style="border-radius: 7px; width: 300px;
                                height: 30px; font-size:larger;">
                                 <asp:RadioButton ID="rb_instuveh" runat="server" RepeatDirection="Horizontal" GroupName="vehicletype"
                        Text="Institution Vehicle" onchange="return rbvehicletype();" Checked="true">
                    </asp:RadioButton>
                    <asp:RadioButton ID="rb_otherveh" runat="server" RepeatDirection="Horizontal" GroupName="vehicletype"
                        Text="Others" onchange="return rbvehicletype();"></asp:RadioButton>
                         </div>
                         </td>
                        <%--Date & Time--%>
                        <td align="right">
                            <div class="maindivstyle" align="center" style="border-radius: 7px; width: 300px;
                                height: 40px; font-size:large;">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_vehicledate" runat="server" Style="top: 10px; left: 6px;" Text="Date"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="upp_vehicledate" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_vehicledate" runat="server" CssClass="txtcaps textbox1 txtheight"
                                                         OnTextChanged="txt_satffdate_TextChanged" ReadOnly="true" AutoPostBack="true"></asp:TextBox>
                                                   <%-- <asp:CalendarExtender ID="CalendarExtender7" TargetControlID="txt_vehicledate" runat="server"
                                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                    </asp:CalendarExtender>--%>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_vehicletime" runat="server" Style="top: 10px; left: 6px;" Text="Time"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_vehicletime" runat="server" CssClass="txtcaps textbox1 txtheight"
                                                OnTextChanged="txt_stafftime_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                   </tr>
               </table>
                
                <div id="div_instvehicle" runat="server" style="display: none;">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_vehicleno2" runat="server" Style="top: 10px; left: 6px;" Text="Vehicle No"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_vehicleno2" runat="server" CssClass="txtcaps txtheight2" onchange="return checkvehid(this.value)" onkeyup="return checkvehid(this.value)"
                                    onblur="return getvehicledetail(this.value)" onfocus="return myFunction(this)" MaxLength="15"></asp:TextBox>
                                    <span style="color: Red;">*</span>
                                <span id="vehmsg"></span>
                                <asp:AutoCompleteExtender ID="acext_vehicleno2" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="getvehicle" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_vehicleno2"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="panelbackground">
                                </asp:AutoCompleteExtender>
                            </td>
                             <td>
                                <asp:Label ID="lbl_rut" runat="server" Style="top: 10px; left: 6px;" Text="Route"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_rut" runat="server" CssClass="txtcaps txtheight"  BackColor="#ffffcc"></asp:TextBox>
                            </td>
                            <td rowspan="3" align="right">
                                <asp:Image ID="image11" runat="server" ImageUrl="" ToolTip="Vehicle/Driver Photo" Style="height: 100px; width: 130px;" />
                            </td>
                        </tr>
                        
                        <tr>
                         <td>
                                <asp:Label ID="lbl_expctdate" runat="server" Style="top: 10px; left: 6px;" Text="Expected Date&Time"></asp:Label>
                            </td>
                            <td>
                                    <asp:TextBox ID="txt_expctdate" runat="server" CssClass="txtcaps txtheight" OnTextChanged="txt_expctdate_TextChanged"
                                            AutoPostBack="true"></asp:TextBox>
                                    &nbsp;
                                <asp:TextBox ID="txt_expcttime" runat="server" CssClass="txtcaps txtheight" OnTextChanged="txt_expcttime_TextChanged"
                                    AutoPostBack="true"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lbl_driver1" runat="server" Style="top: 10px; left: 6px;" Text="Driver"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_driver1" runat="server" CssClass="txtcaps txtheight3"  BackColor="#ffffcc"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                         <td>
                            </td>
                            <td>
                                
                            </td>
                            <td>
                                <asp:Label ID="lbl_insurstatus" runat="server" Style="top: 10px; left: 6px;" Text="Insurance status"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_insurstatus" runat="server" CssClass="txtcaps txtheight"  BackColor="#ffffcc"></asp:TextBox>
                            </td>
                            </tr>
                            <tr>
                             <td>
                                <asp:Label ID="lbl_purpos1" runat="server" Style="top: 10px; left: 6px;" Text="Purpose"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_purpos1" runat="server" CssClass="txtcaps txtheight5" onkeyup="myFunCaps(this.id)"></asp:TextBox>
                                 <asp:FilteredTextBoxExtender ID="ftext_purpos1" runat="server" TargetControlID="txt_purpos1"
                                        FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .,-&">
                                    </asp:FilteredTextBoxExtender>
                            </td>
                            <td>
                                <asp:Label ID="lbl_fcstatus" runat="server" Style="top: 10px; left: 6px;" Text="FC status"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_fcstatus" runat="server" CssClass="txtcaps txtheight"  BackColor="#ffffcc"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                        <td>
                          <asp:Label ID="lbl_approvedstaus" runat="server" Style="top: 10px; left: 6px;" Text="Approved Status"></asp:Label>
                        </td>
                            <td>
                                <asp:RadioButton ID="rb_appstyes" runat="server" RepeatDirection="Horizontal" GroupName="yes"
                                    Text="Yes" onchange="return vehicelapstatus();" Checked="true"></asp:RadioButton>
                                <asp:RadioButton ID="rb_appstno" runat="server" RepeatDirection="Horizontal" GroupName="yes"
                                    Text="No" onchange="return vehicelapstatus();"></asp:RadioButton>
                            </td>
                            <td>
                                <asp:Label ID="lbl_licstatus" runat="server" Style="top: 10px; left: 6px;" Text="License Status"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_licstatus" runat="server" CssClass="txtcaps txtheight"  BackColor="#ffffcc"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td colspan="2">
                                <div id="appstatus_yes" runat="server" style="display: none;">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_personname" runat="server" Style="top: 10px; left: 6px;" Text="Person Name"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_personname" runat="server" CssClass="txtcaps txtheight2" Width="250px"
                                                    onblur="return getvehiappdet(this.value)"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="ftext_personname" runat="server" TargetControlID="txt_personname"
                                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-&()">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:AutoCompleteExtender ID="acext_personname" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="getstaffnamewithdept" MinimumPrefixLength="0" CompletionInterval="100"
                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_personname"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    CompletionListItemCssClass="panelbackground">
                                                </asp:AutoCompleteExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_depart" runat="server" Style="top: 10px; left: 6px;" Text="Department"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_depart" runat="server" CssClass="txtcaps" Width="200px" BackColor="#ffffcc"></asp:TextBox>
                                            </td>
                                            </tr>
                                            <tr>
                                            <td>
                                                <asp:Label ID="lbl_design" runat="server" Style="top: 10px; left: 6px;" Text="Designation"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_design" runat="server" CssClass="txtcaps txtheight4" BackColor="#ffffcc"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="div_othervehicle" runat="server" style="display: none;">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_vehicleno1" runat="server" Style="top: 10px; left: 6px;" Text="Vehicle No"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_vehicleno1" runat="server" CssClass="txtcaps txtheight2" onfocus="return myFunction(this)" MaxLength="15"></asp:TextBox>
                                 <asp:FilteredTextBoxExtender ID="ftext_vehicleno1" runat="server" TargetControlID="txt_vehicleno1"
                                                    FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" -">
                                                </asp:FilteredTextBoxExtender>
                                <span style="color: Red;">*</span>
                            </td>
                            <td colspan="2" rowspan="3">
                                <asp:Image ID="image12" runat="server" ImageUrl="" ToolTip="Vehicle/Person's Photo" Style="height: 110px; width: 130px;" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_brotname" runat="server" Style="top: 10px; left: 6px;" Text="Brought By Name"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_brotname" runat="server" CssClass="txtcaps txtheight4"></asp:TextBox>
                                 <asp:FilteredTextBoxExtender ID="ftext_brotname" runat="server" TargetControlID="txt_brotname"
                                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-&">
                                                </asp:FilteredTextBoxExtender>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                        
                        <tr>
                            <td>
                                <asp:Label ID="lbl_mblno1" runat="server" Style="top: 10px; left: 6px;" Text="Mobile No"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_mblno1" runat="server" CssClass="txtcaps txtheight2" MaxLength="10"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="ftext_mblno1" runat="server" TargetControlID="txt_mblno1"
                                                        FilterType="numbers" ValidChars="">
                                                    </asp:FilteredTextBoxExtender>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_purpose" runat="server" Style="top: 10px; left: 6px;" Text="Purpose"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_purpose" runat="server" CssClass="txtcaps txtheight5" onkeyup="myFunCaps(this.id)"></asp:TextBox>
                                 <asp:FilteredTextBoxExtender ID="ftext_purpose" runat="server" TargetControlID="txt_purpose"
                                        FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .,-&">
                                    </asp:FilteredTextBoxExtender>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:Button ID="btn_vehicleok" runat="server" CssClass="textbox btn2" Text="Save" OnClientClick="return validvehicle()" OnClick="btn_vehicleok_Click" />
                <asp:Button ID="btn_vehicleclear" runat="server" CssClass="textbox btn2" Text="Clear"
                    OnClientClick="return btnvehicleclear();" />
            </div>
              <center>
    
            </center> 
    
            </center> 
             </div>
        <%-----------end of div_vehicle----------%>
    </center>
   <div style="height: 1px; width: 1px; overflow: auto;">
        <div id="contentDiv" runat="server" style="height: auto; width: 900px;" visible="false">
        </div>
    </div>
     <input type="hidden" runat="server" id="Hidden1"  />
      <input type="hidden" runat="server" id="Hidden2"  />
       <input type="hidden" runat="server" id="Hidden3"  />
          <input type="hidden" runat="server" id="roll"  />
        <asp:Label ID="Label2" runat="server" Style="top: 10px; left: 6px;" Visible="false"></asp:Label>
         <td>
                            <asp:Label ID="lbl_phno" runat="server" Style="top: 10px; left: 6px;" Text="Phone No" Visible="false"></asp:Label>
                        </td>
          <td>
                            <asp:TextBox ID="txt_phno" runat="server" CssClass="txtcaps txtheight2" MaxLength="15"  Width="0px" Visible="false"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="ftext_phno" runat="server" TargetControlID="txt_phno"
                                FilterType="numbers" ValidChars="">
                            </asp:FilteredTextBoxExtender>
                             
                        </td>
       
    </div> 
       
</asp:Content>
