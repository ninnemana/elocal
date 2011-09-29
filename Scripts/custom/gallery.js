var thumb_width = 0;
$(function () {
    $.each($('#thumbs ul li img').get(), function (i, img) {
        thumb_width += 117;
    });
    thumb_width = ($('#thumbs ul li img').get().length * 117);
    $('#thumbs ul').css('width', thumb_width + 'px');

    var first_src = $('#thumbs li img:first').attr('src');
    var first_title = $('#thumbs li img:first').attr('alt');
    if (first_title == undefined) { first_title = ''; }
    $('#main').html('<p>' + first_title + '</p><img src="' + first_src + '" />');

    $('#thumbs li img').live('click', function () {
        var src = $(this).attr('src');
        var title = $(this).attr('alt');
        $('#main').find('img').fadeOut('slow', function () {
            $('#main').find('p').text(title);
            $('#main').find('img').attr('src', src);
            $('#main').find('img').fadeIn();
        });
    });

    $('.arrow_right').click(function () {
        var left_pos = parseFloat($('#thumb_container').find('ul').css('left'));
        if (isNaN(left_pos)) {
            $('#thumb_container').find('ul').animate({ left: "-124px" });
        } else {
            new_pos = parseFloat(left_pos);
            new_pos = new_pos - 124;
            if (Math.abs(new_pos) < (thumb_width - 500)) {
                $('#thumb_container').find('ul').animate({ left: new_pos + "px" });
            }
        }
    });

    $('.arrow_left').click(function () {
        var right_pos = parseFloat($('#thumb_container').find('ul').css('left'));
        if (isNaN(right_pos)) {
            // DO NOTHING, maybe add a ! in front of isNaN()?
        } else {
            new_pos = parseFloat(right_pos);
            new_pos = new_pos + 124;
            if (Math.abs(new_pos) < thumb_width && new_pos < 1) {
                $('#thumb_container').find('ul').animate({ left: new_pos + "px" });
            }
        }
    });

});