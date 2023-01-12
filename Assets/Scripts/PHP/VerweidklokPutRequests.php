<?php
	include 'db_connect.php';

	$result = "";
	$notFound = false;
	$missingFields = false;

	if ($_GET["request"] == 'UpdateSheep')
	{
		if (isset($_GET["Sheep_UUID"]) and isset($_GET["Farmer_UUID"]))
		{
		    $result = $mysqli->query("UPDATE `VerweidklokSheepTable` SET Sheep_Label = '".$_GET["Sheep_Label"]."', Sheep_Female = '".$_GET["Sheep_Female"]."'"
				." WHERE Sheep_UUID = '".$_GET["Sheep_UUID"]."' AND Farmer_UUID = '".$_GET["Farmer_UUID"]."';");
		}
		else $missingFields = true;
	}
	else
	{
		$notFound = true;
		echo("Request not found! Request: ".$_GET["request"]);
	}

	if ($missingFields == true)
	{
		$notFound = true;
	    echo("Not all fields set!");
	}

	if ($notFound == false)
	{
		$dataArray = array();
	    while($row = mysqli_fetch_assoc($result))
	    {
	    	$dataArray[] = $row;
	    }
	    echo json_encode($dataArray);
	}

	$mysqli->close();
?>
