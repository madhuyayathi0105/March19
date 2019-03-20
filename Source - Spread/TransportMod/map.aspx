<%@ Page Title="" Language="C#" MasterPageFile="~/TransportMod/TransportSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="map.aspx.cs" Inherits="map" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <style type="text/css">
        html
        {
            height: 100%;
        }
        body
        {
            height: 100%;
            margin: 0;
            padding: 0;
        }
        #map_canvas
        {
            height: 100%;
        }
    </style>
   <%-- <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js">
    </script>
   --%>
   
    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyC6v5-2uaq_wusHDktM9ILcqIrlPtnZgEk&sensor=false"></script>

    <script type="text/javascript">

    function initialize() {
        var vehiddumy = "";
        var markers = JSON.parse('<%=ConvertDataTabletoString() %>');
//        alert('sadffd');
        var mapOptions = {
            center: new google.maps.LatLng(markers[0].lat, markers[0].lng),
            zoom: 16,
            animation: google.maps.Animation.DROP,

            mapTypeId: google.maps.MapTypeId.ROADMAP
            //  marker:true
        };
        var infoWindow = new google.maps.InfoWindow({
            
            maxWidth: 200
        });

        var map = new google.maps.Map(document.getElementById("map_canvas"), mapOptions);
        for (i = 0; i < markers.length; i++) {
            var data = markers[i];


            var myLatlng = new google.maps.LatLng(data.lat, data.lng);
            var iconBase = 'mapmaker.png';


            if (i == 0) {
                var marker = new google.maps.Marker({
                    position: myLatlng,
                    zoom: 16,
                    animation: google.maps.Animation.DROP,

                    mapTypeId: google.maps.MapTypeId.ROADMAP,
                    map: map,
                    icon: iconBase,

                    title: data.title
                });
            }
            else {
                var marker = new google.maps.Marker({
                    position: myLatlng,
                    zoom: 16,
                    animation: google.maps.Animation.DROP,

                    mapTypeId: google.maps.MapTypeId.ROADMAP,
                    map: map,
                   

                    title: data.title
                });
            }
           

//            var langlatsri = [];
//                   var sricheck="";
//                                for (var i in markers) {

//                                    var item = markers[i];
//                                     
//                                    langlatsri.push({
//                                        "lat": item.lat,
//                                        "lng": item.lng,                                        
//                                       
//                                    });
//                                }

//                              alert(langlatsri[0].value);
//                       var flightPath = new google.maps.Polyline({
//                path: langlatsri,
//                geodesic: true,
//                strokeColor: '#FF0000',
//                strokeOpacity: 1.0,
//                strokeWeight: 2
//              });

//              flightPath.setMap(map);

             
           
            (function (marker, data) {


                // Attaching a click event to the current marker
                //                google.maps.event.addListener(marker, 'click', toggleBounce);

                google.maps.event.addListener(marker, "click", function (e) {
                    var vehid = data.VehicleID;

                    var speed = data.Speed;
                    var loc = data.GoogleLocation;
                    var noofstud = data.noofstud;
                    // alert(noofstud);
                    var contentnew = "";
                    if (vehid != "") {
                        vehiddumy = vehid;
                        contentnew = "Vehicle Id :" + vehid + "<br/>" + "No of students : " + noofstud + "<br/>" + "Speed : " + speed + "<br/>" + "Location : " + loc;
                    }
                    else {
                        contentnew = "Vehicle Id :  TN 123 "+"<br/>" + "Route Id :" + speed + "<br/>" + "Stage : " + loc;
                    }

                    //alert(contentnew);
                    infoWindow.setContent(contentnew);


                    infoWindow.open(map, marker);
                    //                    marker.setAnimation(google.maps.Animation.BOUNCE);

                });
            })(marker, data);

           

           


//            var map = new google.maps.Map(document.getElementById("map_canvas"), mapOptions);
//        for (i = 0; i < markers.length; i++) {
//            var data = markers[i];
//         
//           
//            var myLatlng = new google.maps.LatLng(data.lat, data.lng);
//            var marker = new google.maps.Marker({
//                position: myLatlng,
//                zoom: 16,
//               

//    mapTypeId:google.maps.MapTypeId.ROADMAP,
//                map: map,
//                
//            });

        }
//        var markers123 = JSON.parse('<%=ConvertDataTabletoString() %>');
//        var langlatsri = [];
//       var sricheck="";
//                    for (var i in markers123) {

//                        var item = markers123[i];
//                         sricheck=item.lng;
//                        langlatsri.push({
//                            "lat": item.lat,
//                            "lng": item.lng,
//                            
//                           
//                        });
//                    }
                  
//           var flightPath = new google.maps.Polyline({
//    path: langlatsri,
//    geodesic: true,
//    strokeColor: '#FF0000',
//    strokeOpacity: 1.0,
//    strokeWeight: 2
//  });

//  flightPath.setMap(map);

    }
//    setInterval(initialize, 20000);


//    google.maps.event.addDomListener(window, 'load', initialize);
    //  



    setTimeout(function () { location.reload() }, 20000);


   
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <body onload="initialize()">
                <table style="margin-top: 187px;">
                    <tr>
                        <td>
                            <asp:Label ID="errmsg" runat="server" Style="margin-top: 250px; font-family: Book Antiqua;
                                color: Red; font-weight: bold; font-size: medium"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="map_canvas" style="width: 1000px; height: 500px">
                            </div>
                        </td>
                    </tr>
                </table>
                <%--<div id="drr" class="sdd" style=" height:150px; width:200px; background-color:Red"></div>--%>
            </body>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
