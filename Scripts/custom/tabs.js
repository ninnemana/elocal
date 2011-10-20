$(document).ready(function () {

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

    jQuery('.alt_img').live('mouseover', function () {
        var alt_src = jQuery(this).attr('src');
        var main_image = jQuery(this).parent().parent().parent().parent().find('.main_img');
        var main_src = jQuery(main_image).attr('src');

        jQuery(this).parent().attr('href', main_src);
        jQuery(this).attr('src', main_src);

        jQuery(main_image).parent().attr('href', alt_src);
        jQuery(main_image).attr('src', alt_src);
    });

    jQuery('.alt_img').error(function () {
        jQuery(this).parent().remove();
    });

    var tab_contents = jQuery('.horizontal_tab_content').get();
    jQuery.each(tab_contents, function (i, content) {
        // Get the part listings in this tab
        var listings = jQuery(this).find('.part_listing');
        if (listings.length > 5) { // We have more than 5 part listings for this tab
            var i = 5;
            while (i < listings.length) {
                jQuery(listings[i]).hide();
                i++;
            }
            var page_count = Math.ceil(listings.length / 5);
            var paging_html = '<div style="clear:both"></div><span class="paging">';
            var starting_pos = 0;
            var ending_pos = 4;
            for (var i = 1; i < page_count + 1; i++) {
                if (i == 1) {
                    paging_html += '<span class="tab_page active" id="' + starting_pos + '_' + ending_pos + '">' + i + '</span>';
                } else {
                    paging_html += '<span class="tab_page" id="' + starting_pos + '_' + ending_pos + '">' + i + '</span>';
                }
                starting_pos += 5;
                ending_pos += 5;
            }
            paging_html += '</span>';
            jQuery('.part_tools').find('.bottom_tools').append(paging_html);
        }
    });


    jQuery('.tab_page').live('click', function () {
        var clicked_page = jQuery(this).text();
        var positionsArr = jQuery(this).attr('id').split('_');
        var starting_pos = positionsArr[0];
        var ending_pos = positionsArr[1];
        jQuery('.tab_page').removeClass('active');
        jQuery(this).addClass('active');
        jQuery(this).closest(".horizontal_tab_content").find(".part_listing").hide();
        var listings = jQuery(this).closest(".horizontal_tab_content").find(".part_listing");
        jQuery.each(listings, function (i, listing) {
            if (i >= starting_pos && i <= ending_pos) {
                jQuery(listing).show();
            }
        });

    });

});