jQuery(document).ready(function () {

    // Show all the drop downs
    jQuery('#getMake').remove();
    jQuery('#getModel').remove();
    jQuery('#getStyle').remove();
    jQuery('#select_make').show().attr('disabled', 'disabled');
    jQuery('#select_model').show().attr('disabled', 'disabled');
    jQuery('#select_style').show().attr('disabled', 'disabled');

    //if (jQuery.browser.msie == undefined) {

    if ($.browser.msie && $.browser.version < 9) {
        $('.lookup_selector select').live('focus', function () {
            $(this).data('orig_width', $(this).css('width')).css('width', 'auto');
        }).live('blur', function () {
            $(this).css('width', $(this).data('orig_width'));
        });
    }

    jQuery.each($('#hor_lookup').find('select'), function (select, i) {
        var first_option = $(this).find('option[value="0"]').text();
        var select = $(this).clone();
        var name = $(this).attr('name');
        var id = $(this).attr('id');
        var style = $(this).attr('style');
        var new_html = '<div class="lookup_selector"><span>' + first_option + '</span><select name="' + name + '" id="' + id + '" style="' + style + '">' + $(select).html() + '</select></div>';
        $(this).after(new_html);
        $(this).remove();
    });

    // Handle the change of year
    jQuery('#select_year').live('change', function () {
        var year = jQuery(this).val();
        $(this).parent().find('span').text($(this).val());
        jQuery('#select_make').html('<option value="0">- Select Make -</option>').attr('disabled', 'disabled');
        jQuery('#select_make').parent().find('span').text('- Select Make -');
        jQuery('#select_model').html('<option value="0">- Select Model -</option>').attr('disabled', 'disabled');
        jQuery('#select_model').parent().find('span').text('- Select Model -');
        jQuery('#select_style').html('<option value="0">- Select Style -</option>').attr('disabled', 'disabled');
        jQuery('#select_style').parent().find('span').text('- Select Style -');
        if (year > 0) {
            jQuery.get('http://docs.curthitch.biz/API/GetMake?dataType=JSONP&callback=loadMake', { 'year': year }, function () { }, 'jsonp');
        }
    });

    // Handle the change of make
    jQuery('#select_make').live('change', function () {
        var year = jQuery('#select_year').val();
        var make = jQuery('#select_make').val();
        $(this).parent().find('span').text($(this).val());
        jQuery('#select_model').html('<option value="0">- Select Model -</option>').attr('disabled', 'disabled');
        jQuery('#select_model').parent().find('span').text('- Select Model -');
        jQuery('#select_style').html('<option value="0">- Select Style -</option>').attr('disabled', 'disabled');
        jQuery('#select_style').parent().find('span').text('- Select Style -');
        if (year > 0) {
            jQuery.get('http://docs.curthitch.biz/API/GetModel?dataType=JSONP&callback=loadModel', { 'year': year, 'make': make }, function () { }, 'jsonp');
        }
    });

    // Handle the change of model
    jQuery('#select_model').live('change', function () {
        var year = jQuery('#select_year').val();
        var make = jQuery('#select_make').val();
        var model = jQuery(this).val();
        $(this).parent().find('span').text($(this).val());
        jQuery('#select_style').html('<option value="0">- Select Style -</option>').attr('disabled', 'disabled');
        jQuery('#select_style').parent().find('span').text('- Select Style -');
        if (year > 0) {
            jQuery.get('http://docs.curthitch.biz/API/GetStyle?dataType=JSONP&callback=loadStyle', { 'year': year, 'make': make, 'model': model }, function () { }, 'jsonp');
        }
    });

    // Show find hitch button
    jQuery('#select_style').live('change', function () {
        $(this).parent().find('span').text($(this).val());
        jQuery('#getHitch').show();
    });
    /*} else {

    // Handle the change of year
    jQuery('#select_year').live('change', function () {
    var year = jQuery(this).val();
    jQuery('#select_make').html('<option value="0">- Select Make -</option>').attr('disabled', 'disabled');
    jQuery('#select_model').html('<option value="0">- Select Model -</option>').attr('disabled', 'disabled');
    jQuery('#select_style').html('<option value="0">- Select Style -</option>').attr('disabled', 'disabled');
    if (year > 0) {
    jQuery.get('http://docs.curthitch.biz/API/GetMake?dataType=JSONP&callback=loadMake', { 'year': year }, function () { }, 'jsonp');
    }
    });

    // Handle the change of make
    jQuery('#select_make').live('change', function () {
    var year = jQuery('#select_year').val();
    var make = jQuery('#select_make').val();
    jQuery('#select_model').html('<option value="0">- Select Model -</option>').attr('disabled', 'disabled');
    jQuery('#select_style').html('<option value="0">- Select Style -</option>').attr('disabled', 'disabled');
    if (year > 0) {
    jQuery.get('http://docs.curthitch.biz/API/GetModel?dataType=JSONP&callback=loadModel', { 'year': year, 'make': make }, function () { }, 'jsonp');
    }
    });

    // Handle the change of model
    jQuery('#select_model').live('change', function () {
    var year = jQuery('#select_year').val();
    var make = jQuery('#select_make').val();
    var model = jQuery(this).val();
    jQuery('#select_style').html('<option value="0">- Select Style -</option>').attr('disabled', 'disabled');
    if (year > 0) {
    jQuery.get('http://docs.curthitch.biz/API/GetStyle?dataType=JSONP&callback=loadStyle', { 'year': year, 'make': make, 'model': model }, function () { }, 'jsonp');
    }
    });

    // Show find hitch button
    jQuery('#select_style').live('change', function () {
    jQuery('#getHitch').show();
    });
    }*/

    jQuery('#resetLookup').live('click', function () {
        jQuery('#select_year').val(0);
        jQuery('#select_make').html('<option value="0">- Select Make -</option>').attr('disabled', 'disabled');
        jQuery('#select_model').html('<option value="0">- Select Model -</option>').attr('disabled', 'disabled');
        jQuery('#select_style').html('<option value="0">- Select Style -</option>').attr('disabled', 'disabled');
        return false;
    });
});


function loadMake(makes) {
    jQuery.each(makes, function (i, make) {
        jQuery('#select_make').append('<option>' + make + '</option>');
    });
    jQuery('#select_make').removeAttr('disabled');
}

function loadModel(models) {
    jQuery.each(models, function (i, model) {
        jQuery('#select_model').append('<option>' + model + '</option>');
    });
    jQuery('#select_model').removeAttr('disabled');
}

function loadStyle(styles) {
    jQuery.each(styles, function (i, style) {
        jQuery('#select_style').append('<option>' + style + '</option>');
    });
    jQuery('#select_style').removeAttr('disabled');
}