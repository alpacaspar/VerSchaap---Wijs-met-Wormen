<?php
	include 'db_connect.php';

	if ($_GET["request"] == 'UpdateSheep')
	{
	    $result = $mysqli->query("UPDATE `VerweidklokSheepTable` SET Sheep_Label = '".$_GET["Sheep_Label"]."', Sheep_Female = '".$_GET["Sheep_Female"]."'"
			." WHERE Sheep_UUID = '".$_GET["Sheep_UUID"]."' AND Farmer_UUID = '".$_GET["Farmer_UUID"]."';");

	    while($row = mysqli_fetch_assoc($result))
	    {
	        echo($row);
	    }
	}
	else
	{
		echo("Request not found! Request: ".$_GET["request"]);
	}

	$mysqli->close();
?>
