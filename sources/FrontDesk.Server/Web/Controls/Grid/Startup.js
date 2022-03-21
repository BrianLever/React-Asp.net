<script language="JavaScript">
	var {--HiddenFieldName--} = (document.getElementsByName("{--HiddenFieldName--}")[0]).value.split(", ");
	for(var index = 0; index < {--HiddenFieldName--}.length; index++)
	{
		if({--HiddenFieldName--}[index] != null && {--HiddenFieldName--}[index] != "")
			HierarGrid_toggleRow(document.getElementsByName({--HiddenFieldName--}[index])[0]);
	}
</script>
