<?php
	include 'db_connect.php';

	if ($_GET['request'] == 'AddSheep')
	{
	    $result = $mysqli->query("INSERT INTO `VerweidklokSheepTable` ('Sheep_UUID', 'Sheep_Label', 'Sheep_Female', 'Farmer_UUID')"
			." VALUES ('".$_GET["Sheep_UUID"]."','".$_GET["Sheep_Label"]."','".$_GET["Sheep_Female"]."','".$_GET["Farmer_UUID"]."');");

	    echo("INSERT INTO `VerweidklokSheepTable` ('Sheep_UUID', 'Sheep_Label', 'Sheep_Female', 'Farmer_UUID')"
			." VALUES ('".$_GET["Sheep_UUID"]."','".$_GET["Sheep_Label"]."','".$_GET["Sheep_Female"]."','".$_GET["Farmer_UUID"]."');");

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
