var map;
var start_lat;
var start_lon;
var locations;
var location_markers = new Array();
jQuery(document).ready(function () {
    locations = eval($('#locations_json').text());
    get_location();
    $('.input_box, .textarea_box').find('.sliding_label').animate({ top: '32px' }, 0);

    $('#name').keydown(function () {
        var str = $(this).val();
        var newstr = str.replace(/\d+/g, '');
        $(this).val(newstr);
    });

    $('#phone').keydown(function () {
        var str = $(this).val();
        var newstr = str.replace(/^[0-9]$/, '');
        $(this).val(newstr);
    });

    // We need to swap out our select box for the pretty version.
    var location_select = $('#location').get()[0];
    var loc_html = '<div class="lookup_selector"><span>- Select Location -</span><select style="padding:0px;text-shadow:none" name="location" id="location">' + $(location_select).html() + '</select></div>';
    $(location_select).after(loc_html);
    $(location_select).remove();

    $('#location').live('change', function () {
        $(this).parent().find('span').text($(this).find('option[value="' + $(this).val() + '"]').text());
        var loc_id = $(this).val();
        load_location(loc_id);
    });

    $('input[type=text], input[type=email], textarea').focus(function () {
        $(this).parent().prev().animate({ top: '10px' }, 1000);
    });

    $('input[type=text], input[type=email], textarea').blur(function () {
        $(this).parent().prev().animate({ top: '32px' }, 500);
    });

    jQuery('#contact_form input[type=submit]').live('click', function () {
        jQuery('input,textarea').removeClass('required');
        jQuery('.message').text('');
        var errors = 0;
        var name = jQuery('#name').val();
        var email = jQuery('#email').val();
        var phone = jQuery('#phone').val();
        var subject = jQuery('#subject').val();
        var message = jQuery('#message').val();
        var contact_type = "";

        /* Validate Name */
        if (name.length == 0) {
            errors++;
            jQuery('#name').addClass('required');
        }

        /* Validate Subject */
        if (subject.length == 0) {
            errors++;
            jQuery('#subject').addClass('required');
        }

        /* Validate Message Body */
        if (message.length == 0) {
            errors++;
            jQuery('#message').addClass('required');
        }

        /* Validate Method of Contact */
        if (jQuery('#contact_type_phone').is(':checked')) { contact_type = "phone"; }
        if (jQuery('#contact_type_email').is(':checked')) { contact_type = "email"; }
        if (contact_type.length == 0) {
            errors++;
            jQuery('input[type=radio]').addClass('required');
        } else if (contact_type == "phone") {
            if (phone.length == 0) {
                errors++;
                jQuery('#phone').addClass('required');
            }
        } else if (contact_type == "email") {
            if (email.length == 0) {
                errors++;
                jQuery('#email').addClass('required');
            }
        }
        if (errors > 0) {

            showMessage("Please correct the errors and retry.");
            return false;
        } else {
            return true;
        }
    });

});

// Check whether the clients browser supports the geolocation API
function get_location() {
    if (Modernizr.geolocation) {
        navigator.geolocation.getCurrentPosition(initiate_geomap,initiate_map);
    } else {
        // No native support, falling back
        initiate_map();
    }
}

// 
function initiate_geomap(pos) {
    start_lat = pos.coords.latitude;
    start_lon = pos.coords.longitude;

    var latLng = new google.maps.LatLng(start_lat, start_lon);
    var mapOptions = {
        zoom: 6,
        center: latLng,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    map = new google.maps.Map(document.getElementById("map_canvas"), mapOptions);
    drop_markers();
}

function initiate_map() {
    var latLng = new google.maps.LatLng(39.8, -98.5);
    var mapOptions = {
        zoom: 2,
        center: latLng,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    map = new google.maps.Map(document.getElementById("map_canvas"), mapOptions);
    drop_markers();
}

function drop_markers() {
    location_markers = new Array();
    jQuery.each(locations, function (i, location) {
        var latLng = new google.maps.LatLng(location.latitude, location.longitude);
        var marker = new google.maps.Marker({
            position: latLng,
            map: map,
            title: location.name,
            animation: google.maps.Animation.DROP
        });
        location_markers[location.locationID] = marker;
    });
}

function load_location(id) {
    // Hide any information that could have been displayed previously
    $('#phone_number').hide();
    $('#fax_number').hide();
    $('#services').find('table').html('');
    $('#services').hide();

    // Execute AJAX to retrieve Location
    $.getJSON('/Contact/GetLocation/' + id, function (resp) {
        if (resp.error == undefined) { // Error checking

            // Remove the preloader
            $('#preloader').remove();

            // Make sure we have a valid phone number
            if (resp.phone != null && resp.phone != undefined && resp.phone.length > 0) {
                $('#phone_number').find('.text').text(resp.phone);
                $('#phone_number').show();
            }

            // Make sure we have a valid fax number
            if (resp.fax != null && resp.fax != undefined && resp.fax.length > 0) {
                $('#fax_number').find('.text').text(resp.fax);
                $('#fax_number').show();
            }

            // Make sure there are services for this location
            if (resp.services.length > 0) {
                $.each(resp.services, function (i, service) {
                    var html = '<tr><td>' + service.service_title + '</td><td>$' + service.service_price;
                    if (service.hourly == 1) { html += '/hr'; }
                    html += '</td></tr>';
                    if (service.description.length > 0) {
                        html += '<tr><td colspan="2" class="desc">' + service.description + '</td></tr>';
                    }
                    $('#services').find('table').append(html);
                });
                $('#services').show();
            }

            if (map == undefined) {
                initiate_map();
            }

            // Pan the map to the point of the location
            map.setZoom(13);
            map.setCenter(new google.maps.LatLng(resp.latitude, resp.longitude));
        } else {
            $('#preloader').html('Failed to load location.');
        }
    });
    $('#map_canvas').after('<div style="display:block;width:80%;margin:auto;text-align:center" id="preloader">Loading location...<br /><img src="/Content/img/preloader.gif" /></div>');
}