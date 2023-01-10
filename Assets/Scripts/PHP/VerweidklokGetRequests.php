<?php
	include 'db_connect.php';

	if ($_GET['request'] == 'GetSheep')
	{
		if (isset($_GET["Sheep_UUID"]) and isset($_GET["Farmer_UUID"]))
		{
		    $result = $mysqli->query("SELECT * FROM `VerweidklokSheepTable` WHERE Sheep_UUID = " 
		    	."'".$_GET[Sheep_UUID]."' AND Farmer_UUID = '".$_GET[Farmer_UUID]."';");

		    while($row = mysqli_fetch_assoc($result))
		    {
		        echo($row);
		    }

			$mysqli->close();
		}
		else 
		{
		    echo("Not all fields set!");
		}
	}
	else
	{
		echo("Request not found!");
	}
?>
