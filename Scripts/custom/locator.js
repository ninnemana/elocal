var map;
var locations = '';
var index_locations = new Array();
var markers = new Array();
var info_windows = new Array();
var directionsService;
var directionsDisplay;
var markerArray = new Array();
var stepDisplay;

$(document).ready(function () {
    locations = eval($('#locations_json').text());
    loadMap();
    loadLocations();

    directionsService = new google.maps.DirectionsService();

    $('.location').mouseover(function () {
        $(this).find('.directions').show();
        $(this).find('.view_on_map').show();
    });

    $('.location').mouseleave(function () {
        $(this).find('.directions').hide();
        $(this).find('.view_on_map').hide();
    });

    $('.view_on_map').live('click', function () {
        var loc_id = $(this).next().attr('id');
        var mark = new google.maps.Marker();
        var iWindow = new google.maps.InfoWindow();
        mark = markers[loc_id];
        iWindow = mark['info_window'];
        iWindow.open(map, mark);
    });

    $('.directions').live('click', function () {
        $(this).parent().find('.direction_form').find('#loc_id').val($(this).attr('id'));
        $(this).parent().find('.direction_form').slideDown();
    });

    $('#btnSubmit').live('click', function () {
        var address = $(this).parent().find('#address').val();
        var city = $(this).parent().find('#city').val();
        var state = $(this).parent().find('#state').val();
        var formatted_address = address + ' ' + city + ', ' + state;

        var loc_id = $(this).parent().find('#loc_id').val();
        var lat = eval(index_locations[loc_id]).latitude;
        var lon = index_locations[loc_id].longitude;
        var dLatLon = new google.maps.LatLng(lat, lon);

        var request = {
            origin: formatted_address,
            destination: dLatLon,
            travelMode: google.maps.TravelMode.DRIVING
        };
        directionsService.route(request, function (result, status) {
            if (status == google.maps.DirectionsStatus.OK) {
                directionsDisplay.setDirections(result);
                showSteps(result);
            }
        });
    });


});

function loadMap() {
    // We need to focus on the primary location (if it exists)
    var lat_lng = '';
    directionsDisplay = new google.maps.DirectionsRenderer();
    $.each(locations, function (i, location) {
        if (location.isPrimary == 1) {
            lat_lng = new google.maps.LatLng(location.latitude, location.longitude);
        }
    });

    if (lat_lng != '') {
        lat_lng = new google.maps.LatLng(locations[0].latitude, locations[0].longitude);
    }
    var mapOptions = {
        zoom: 11,
        center: lat_lng,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    map = new google.maps.Map(document.getElementById('map'), mapOptions);
    directionsDisplay.setMap(map);
    stepDisplay = new google.maps.InfoWindow();
}

function loadLocations() {
    $.each(locations, function (i, location) {
        var latlng = new google.maps.LatLng(location.latitude, location.longitude);
        var marker = new google.maps.Marker({
            position: latlng,
            map: map,
            title: location.name,
            animation: google.maps.Animation.DROP
        });
        var info_window = new google.maps.InfoWindow({
            content: location.name
        });
        markers[location.locationID] = marker;
        markers[location.locationID]['info_window'] = info_window;
        info_windows[location.locationID] = info_window;
        google.maps.event.addListener(marker, 'click', function () {
            var loc_id = markers.indexOf(marker);
            var iWindow = new google.maps.InfoWindow();
            iWindow = info_windows[loc_id];
            iWindow.open(map, marker);
        });
        index_locations[location.locationID] = location;
    });
}

function showSteps(directionResult) {
  // For each step, place a marker, and add the text to the marker's
  // info window. Also attach the marker to an array so we
  // can keep track of it and remove it when calculating new
  // routes.
  var myRoute = directionResult.routes[0].legs[0];

  for (var i = 0; i < myRoute.steps.length; i++) {
      var marker = new google.maps.Marker({
        position: myRoute.steps[i].start_point,
        map: map
      });
      attachInstructionText(marker, myRoute.steps[i].instructions);
      markerArray[i] = marker;
  }
}

function attachInstructionText(marker, text) {
  google.maps.event.addListener(marker, 'click', function() {
    stepDisplay.setContent(text);
    stepDisplay.open(map, marker);
  });
}