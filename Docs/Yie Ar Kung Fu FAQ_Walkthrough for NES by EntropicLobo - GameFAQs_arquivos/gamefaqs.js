/* jQuery UI dialog clickoutside */
$.widget( "ui.dialog", $.ui.dialog, {
  options: {
    clickOutside: false, // Determine if clicking outside the dialog shall close it
    clickOutsideTrigger: "" // Element (id or class) that triggers the dialog opening
  },

  open: function() {
    var clickOutsideTriggerEl = $( this.options.clickOutsideTrigger );
    var that = this;

    if (this.options.clickOutside){
      // Add document wide click handler for the current dialog namespace
      $(document).on( "click.ui.dialogClickOutside" + that.eventNamespace, function(event){
        if ( $(event.target).closest($(clickOutsideTriggerEl)).length == 0 && $(event.target).closest($(that.uiDialog)).length == 0){
          that.close();
        }
      });
    }

    this._super(); // Invoke parent open method
  },

  close: function() {
    var that = this;

    // Remove document wide click handler for the current dialog
    $(document).off( "click.ui.dialogClickOutside" + that.eventNamespace );

    this._super(); // Invoke parent close method
  },

});

function submit_login()
{
	if (!$('#login_email').val())
	{
		$('#login_email').effect('highlight', 1000);
		$('#login_email').focus();
		return false;
	}
	if (!$('#login_password').val())
	{
		$('#login_password').effect('highlight', 1000);
		$('#login_password').focus();
		return false;
	}
	return true;
}

function show_signup(from)
{
	if($(document).width() < 500)
		var box_width = "98%";
	else
		var box_width = "60%";
	$.ajax({
		type: 'GET',
        url: '/ajax/user_signup?from=' + from,
        success: function(response)
		{
			if(response=='')
			{
				window.location = ('/user/register.html');
			}
			else
			{
				$('#site_dialog').html(response);
				$('#site_dialog').dialog({	resizable: false, dialogClass: "reg_dialog", closeText: "X", height: "auto", width: box_width, modal: true, open: function(){$('.ui-widget-overlay').bind('click',function(){$('#site_dialog').dialog('close');});} });
			}
        }

    });
    return false;
}
function show_login()
{
	if($(document).width() < 500)
		var box_width = "98%";
	else
		var box_width = "30%";
	$('#login_dialog').dialog({	position: "center", resizable: false, dialogClass: "reg_dialog", closeText: "X", height: "auto", width: box_width, modal: true, open: function(){$('.ui-widget-overlay').bind('click',function(){$('#login_dialog').dialog('close');});} });
	return false;
}

function submit_report()
{
	if(!$("input[name='reason']:checked").val())
	{
		alert('You must select a reason to report this post to the moderation staff.');
		return false;
	}
}

function show_msg_report(mnum,bid,tid,mid,firstmid)
{
	$('.report_dialog input[name="reason"]').prop('checked',false);
	$('.report_dialog input[name="mod_other"]').val('');
	$('.report_dialog input[name="rid"]').val('');
	$('.report_dialog input[name="ignore"]').attr('checked',false);
	$('.report_dialog h2.title').html('Report Message - <span data-msgid="'+mid+'">Post #' + mnum + '</span>');
	$('.report_dialog form[name="report_mod"]').attr('action', '/boards/mod/mod_message?board='+bid+'&topic='+tid+'&message='+mid);

	$('input[name="msgid"]').val(mid);
	if(mid!=firstmid)
	{
		$('#m7').parent().hide();
	}
	else
	{
		$('#m7').parent().show();
	}

	if($(document).width() < 500)
		var box_width = "75%";
	else
		var box_width = "35%";
	$('.report_dialog').dialog({ resizable: false, dialogClass: "reg_dialog", closeText: "X", height: "auto", width: box_width, modal: true, open: function(){$('.ui-widget-overlay').bind('click',function(){$('.report_dialog').dialog('close');});} });
	return false;
}

function show_msg_sticky()
{
	if($(document).width() < 500)
		var box_width = "75%";
	else
		var box_width = "35%";
	$('.sticky_dialog').dialog({ resizable: false, dialogClass: "reg_dialog", closeText: "X", height: "auto", width: box_width, modal: true, open: function(){$('.ui-widget-overlay').bind('click',function(){$('.sticky_dialog').dialog('close');});} });
	return false;
}

function show_mygames_nouser_dialog()
{
	if($(document).width() < 500)
		var box_width = "98%";
	else
		var box_width = "30%";
	$('#mygames_nouser_dialog').dialog({resizable: false, dialogClass: "reg_dialog", closeText: "X", height: "auto", width: box_width, modal: true, open: function(){$('.ui-widget-overlay').bind('click',function(){$('#mygames_nouser_dialog').dialog('close');});} });
	return false;
}

function show_faqsearch_dialog(fixed,mobile)
{
	var module = "";
	var dialogclass = "reg_dialog";
	if(fixed)
		dialogclass += " fixed_dialog";
	if($(document).width() < 500)
		var box_width = "98%";
	else
		var box_width = "30%";
	$('.faqsearch_dialog').dialog({clickOutside: true, clickOutsideTrigger: '.togglesearch', resizable: false, dialogClass: dialogclass, closeText: "X", height: "auto", minHeight: 60, width: box_width, position: { my: "left+15 top", at: "right top+30"}, modal: false, open: function(){$('.ui-widget-overlay').bind('click',function(){$('.faqsearch_dialog').dialog('close');});} });
	if($('ul.faq_results li').length>0)
		$('input[name="faqsearch"]').blur();
	if(mobile==1)
		module = "faq-search-mob";
	else
		module = "faq-search";
	return false;
}

function show_pm_dialog()
{
	$('.pm_dialog input[name="subject"]').val('');
	$('.pm_dialog textarea[name="message"]').val('');

	if($(document).width() < 500)
		var box_width = "75%";
	else
		var box_width = "35%";
	$('.pm_dialog').dialog({ resizable: false, dialogClass: "reg_dialog", closeText: "X", height: "auto", width: box_width, modal: true, open: function(){$('.ui-widget-overlay').bind('click',function(){$('.pm_dialog').dialog('close');});} });
	return false;
}

function show_profile_rules_dialog()
{
	if($(document).width() < 500)
		var box_width = "75%";
	else
		var box_width = "35%";
	$('.profile_rules_dialog').dialog({ resizable: false, dialogClass: "reg_dialog", closeText: "X", height: "auto", width: box_width, modal: true, open: function(){$('.ui-widget-overlay').bind('click',function(){$('.profile_rules_dialog').dialog('close');});} });
	return false;
}

function show_profile_report_dialog()
{
	if($(document).width() < 500)
		var box_width = "75%";
	else
		var box_width = "35%";
	$('.profile_report_dialog').dialog({ resizable: false, dialogClass: "reg_dialog", closeText: "X", height: "auto", width: box_width, modal: true, open: function(){$('.ui-widget-overlay').bind('click',function(){$('.profile_report_dialog').dialog('close');});} });
	return false;
}

function show_gs_video_dialog(id)
{
	$('iframe.gs_video').removeAttr('src');
	$('iframe.gs_video').attr('src','https://www.gamespot.com/videos/embed/'+id+'/?autoplay=1&utm_source=gamefaqs&utm_medium=partner&utm_content=video_player&utm_campaign=gamespace_video');

	if($(document).width() < 500)
		var box_width = "95%";
	else
		var box_width = "680px";
	$('.gs_video_dialog').dialog({ resizable: false,dialogClass: "reg_dialog",closeText: "X",height: "auto",width: box_width,modal: true,close: function(){$('iframe.gs_video').removeAttr('src');},open: function(){$('.ui-widget-overlay').bind('click',function(){$('.gs_video_dialog').dialog('close');$('.gs_video').attr('src','');});} });
	return false;
}

function show_qna_expert_dialog()
{
	if($(document).width() < 500)
		var box_width = "75%";
	else
		var box_width = "35%";
	$('.qna_expert_dialog').dialog({ resizable: false, dialogClass: "reg_dialog", closeText: "X", height: "auto", width: box_width, modal: true, open: function(){$('.ui-widget-overlay').bind('click',function(){$('.qna_expert_dialog').dialog('close');});} });
	return false;
}

function track_event(event_id)
{
		return;
}

function post_click(url, action, key, target_id, target_text)
{
	var newForm = $('<form>', {
		'action': url,
		'method': 'post'
	});
	newForm.append($('<input>', {
		'name': 'key',
		'value': key,
		'type': 'hidden'
	}));
	newForm.append($('<input>', {
		'name': 'action',
		'value': action,
		'type': 'hidden'
	}));
	if(target_id)
	{
		newForm.append($('<input>', {
			'name': 'target_id',
			'value': target_id,
			'type': 'hidden'
		}));
	}
	if(target_text)
	{
		newForm.append($('<input>', {
			'name': 'target_text',
			'value': target_text,
			'type': 'hidden'
		}));
	}
	newForm.appendTo( document.body );
    newForm.submit();
}

function pm_click(url, key, user_name)
{
	var newForm = $('<form>', {
		'action': url,
		'method': 'post'
	});
	newForm.append($('<input>', {
		'name': 'key',
		'value': key,
		'type': 'hidden'
	}));
	newForm.append($('<input>', {
		'name': 'to',
		'value': user_name,
		'type': 'hidden'
	}));
	newForm.appendTo( document.body );
    newForm.submit();
}

function clear_all_notifications()
{
	$.ajax({
		type: 'GET',
        url: '/ajax/notification_clear_all',
        success: function(response)
		{
			if(response=='')
			{
				$('span.notifications').hide();
			}
        }
    });
	return false;
}

function gs_link(a,type,id)
{
	var go = a.getAttribute('href');
	$.ajax({
		type: 'GET',
        url: '/ajax/gs_track?p=' + id + '&t=' + type + '&u=' + encodeURIComponent(go),
        success: function(response)
		{
			window.location.href = go;
        }
    });
	return false;




}