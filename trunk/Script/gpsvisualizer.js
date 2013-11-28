var map;

function RouteInfo(name, infoWindow) {
    var routeName = name;
    var window = infoWindow;
    this.onDetailRequested = function(e) {
        window.setContent(routeName);
        window.setPosition(e.latLng);
        window.open(map);
    };
}
function initialize() {
    var mapOptions = {
        zoom: 8,
        center: new google.maps.LatLng(0, 0),
        mapTypeId: google.maps.MapTypeId.SATELLITE
    };
    map = new google.maps.Map(document.getElementById('gmap_div'),
                mapOptions);
    myInfoWindow = new google.maps.InfoWindow();

    bounds = new google.maps.LatLngBounds();

    this.mapReady();
    map.setCenter(bounds.getCenter());
    map.fitBounds(bounds);
}
function GV_Draw_Track(track_seg) {
    for (var i = 0; i < track_seg.length; i++) {
        var seg = track_seg[i];
        var p1 = new google.maps.LatLng(seg.p1.lat, seg.p1.lon);
        var p2 = new google.maps.LatLng(seg.p2.lat, seg.p2.lon);
        bounds.extend(p1);
        bounds.extend(p2);
        var poly = new google.maps.Polyline({
            path: [p1, p2],
            strokeColor: seg.color,
            strokeOpacity: 1.0,
            strokeWeight: 3,
            map: map
        });
    }
}
function GV_Draw_Marker(m) {
    var p = new google.maps.LatLng(m.lat, m.lon);
    bounds.extend(p);
    var marker = new google.maps.Marker({
        position: p,
        title: m.name,
        icon: m.icon,
        color: m.color
    });
    marker.setMap(map);

    var html = "<div class='mk_info_window'><div class='mk_info_window_name'><b><a target='_blank' href='"
    + m.url
    + "' title=\""
    + m.name
    + "\">"
    + m.name
    + "</a></b></div><div class='mk_info_window_photo'><img class='mk_photo' src='"
    + m.photo
    + "'></div><div class='mk_info_window_desc'>"
    + m.desc
    + "</div></div>";
    var obj = new RouteInfo(html, myInfoWindow);

    google.maps.event.addListener(marker, 'click', obj.onDetailRequested);
}

initialize();

this.mapReady = function() { };
