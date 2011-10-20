$(document).ready(function () {
    var vehicle_table = $('#vehicles').find('table').dataTable({
        'bFilter': false,
        'bInfo': false,
        'bLengthChange': false,
        'bPaginate': false
    });

    //When page loads...
    $(".horizontal_tab_content").hide(); //Hide all content
    $("ul.horizontal_tabs li:first").addClass("active").show(); //Activate first tab
    $(".horizontal_tab_content:first").show(); //Show first tab content

    //On Click Event
    $("ul.horizontal_tabs li").click(function () {

        $("ul.horizontal_tabs li").removeClass("active"); //Remove any "active" class
        $(this).addClass("active"); //Add "active" class to selected tab
        $(".horizontal_tab_content").hide(); //Hide all tab content

        var activeTab = $(this).find("a").attr("href"); //Find the href attribute value to identify the active tab + content
        $(activeTab).fadeIn(); //Fade in the active ID content
        return false;
    });

    $('#mini_images').find('a').mouseover(function () {
        var image_path = $(this).find('img').attr('src').replace('100x75', '300x225');
        var image_link = $(this).attr('href');
        var main_path = $('#main_image').find('a').find('img').attr('src').replace('300x225', '100x75');
        var main_link = $('#main_image').find('a').attr('href');

        // Replace the mini image with the current main
        $(this).attr('href', main_link);
        $(this).find('img').attr('src', main_path);

        // Replace the main image with this mini
        $('#main_image').find('a').attr('href', image_link);
        $('#main_image').find('a').find('img').attr('src', image_path);
    });
});