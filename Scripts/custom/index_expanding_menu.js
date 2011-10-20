var mouseover_messagebox = false;
$(document).ready(function () {

    $('img').error(function () {
        $(this).remove();
    });

    //var content_height = $('#view_content').height();
    //$('#category_listing').css('height', content_height);

    $('.sub_cat').hide();
    $('#hor_nav li:last').css('border-right', '2px solid #575757');

    // Handle expanding menu for categories
    $('.cat_link').live('mouseover', function () {
        var link = $(this);
        $('.parent_cat').removeClass('active');
        $(this).parent().addClass('active');
        var subs = $(this).parent().find('.sub_cat').get();
        if (subs.length > 0 && $(this).parent().parent().attr('class') != 'sub_cat') {
            $('.slideOutMenu').remove();
            var listing_width = $('#category_listing').width() + 2;
            if ($.browser.msie) {
                if ($.browser.version == '7.0') {
                    listing_width = -7;
                } else {
                    listing_width = 180;
                }
            }
            var html = "<div class='slideOutMenu' style='margin-left:" + listing_width + "px'><div class='slideOutMenu_container'>";
            if ($(subs).find('li').length > 5) {
                html += '<div style="float:left;">';
                var j = 1;
                $.each($(subs).find('li'), function (i, sub) {
                    if (j == 6) {
                        html += '</div><div style="float:left;border-left:1px inset #727272;height:200px">';
                        j = 1;
                    }
                    j++;
                    html += $(sub).css('display', 'block !important').css('width', '100% !important').html();
                });
                html += '</div>';
            } else {
                $.each($(subs).find('li'), function (i, sub) {
                    html += $(sub).css('color', 'white').css('float', 'none !important').html();
                });
            }
            html += '</div></div>';
            $(link).parent().append(html);
        }
    });

    $('.slideOutMenu_container').live('mouseleave', function () {
        $(this).parent().fadeOut();
        $('.parent_cat').removeClass('active');
    });

    $('#category_listing').live('mouseleave', function () {
        $('.slideOutMenu').fadeOut();
        $('.parent_cat').removeClass('active');
    });

    //// HIDE ERROR MESSAGES ////
    setTimeout('hideMessage()', 10000);

    $('#btnSearch').live('click', function () {
        var term = $('#search_term').val();
        if (term.length > 0) {

        } else {
            showMessage('You must enter a search term.');
        }
    });


    $('#message_box').mouseover(function () {
        mouseover_messagebox = true;
    }).mouseout(function () {
        mouseover_messagebox = false;
    });

    // Make older browsers recognize the 'placeholder' attribute *HTML5 FIX*
    if (!Modernizr.input.placeholder) {
        $("input").each(function () {
            if ($(this).val() == "" && $(this).attr("placeholder") != "") {
                $(this).val($(this).attr("placeholder"));
                $(this).focus(function () {
                    if ($(this).val() == $(this).attr("placeholder")) $(this).val("");
                });
                $(this).blur(function () {
                    if ($(this).val() == "") $(this).val($(this).attr("placeholder"));
                });
            }
        });

        $("textarea").each(function () {
            if ($(this).val() == "" && $(this).attr("placeholder") != "") {
                $(this).val($(this).attr("placeholder"));
                $(this).focus(function () {
                    if ($(this).val() == $(this).attr("placeholder")) $(this).val("");
                });
                $(this).blur(function () {
                    if ($(this).val() == "") $(this).val($(this).attr("placeholder"));
                });
            }
        });
    }

});

function showMessage(message) {
    $('#message_box').find('span').text(message);
    $('#message_box').fadeIn();
    setTimeout('hideMessage()', 5000);
}

function hideMessage() {
    if (!mouseover_messagebox) {
        $('#message_box').fadeOut();
    } else {
        setTimeout('hideMessage()', 800);
    }
}