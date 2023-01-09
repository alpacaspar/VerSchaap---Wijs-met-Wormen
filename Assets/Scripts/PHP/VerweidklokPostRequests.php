<?php
	include 'db_connect.php';

	if ($_POST['request'] == 'AddSheep')
	{
	    $result = $mysqli->query("INSERT INTO `VerweidklokSheepTable` (Sheep_UUID, Sheep_Label, Sheep_Female, Farmer_UUID)"
			." VALUES ('".$_POST["Sheep_UUID"]."','".$_POST["Sheep_Label"]."','".$_POST["Sheep_Female"]."','".$_POST["Farmer_UUID"]."');");

	    while($row = mysqli_fetch_assoc($result))
	    {
	        echo($row);
	    }
	}
	else
	{
		echo("Request not found!");
	}

	$mysqli->close();
?>
