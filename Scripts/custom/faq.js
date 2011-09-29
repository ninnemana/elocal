$(document).ready(function () {

    $('.answer').hide();


    $('.question').live('click', function () {

        // Hide all answers
        $('.answer').slideUp();

        // Show this questions answer
        if ($(this).next().css('display') == 'none') {
            $(this).next().slideDown();
        }
    });
});