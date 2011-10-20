var mouseover_messagebox = false;
$(document).ready(function () {

    $('img').error(function () {
        $(this).hide();
    });

    $('.sub_cat').hide();
    $('#hor_nav li:last').css('border-right', '2px solid #575757');

    // Handle expanding menu for categories
    $('.cat li a').live('click', function () {
        // Get the subcategories
        var subs = $(this).parent().find('.sub_cat');
        // If we have subcategories ::: toggle their display
        if (subs.length > 0 && $(this).parent().parent().attr('class') != 'sub_cat') {
            $(this).parent().find('ul').slideToggle();
            return false;
        }
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