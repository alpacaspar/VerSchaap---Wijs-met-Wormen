<?php
	include 'db_connect.php';

	$result = "";
	$notFound = false;
	$missingFields = false;

	if ($_GET['request'] == 'AddSheep')
	{
		if (isset($_GET["Sheep_UUID"]) and isset($_GET["Farmer_UUID"]))
		{
		    $result = $mysqli->query("INSERT INTO `VerweidklokSheepTable` (Sheep_UUID, Sheep_Label, Sheep_Female, Farmer_UUID)"
				." VALUES ('".$_GET["Sheep_UUID"]."','".$_GET["Sheep_Label"]."','".$_GET["Sheep_Female"]."','".$_GET["Farmer_UUID"]."');");
		}
		else $missingFields = true;
	}
	if ($_GET['request'] == 'AddLot')
	{
		if (isset($_GET["Lot_UUID"]) and isset($_GET["Farmer_UUID"]))
		{
		    $result = $mysqli->query("INSERT INTO `VerweidklokLotTable` (Lot_UUID, Lot_Name, Lot_Surface, Lot_Quality, Lot_Mowed_TS, Lot_State_ID, Farmer_UUID)"
				." VALUES ('".$_GET["Lot_UUID"]."','".$_GET["Lot_Name"]."','".$_GET["Lot_Surface"]."','".$_GET["Lot_Quality"]."','".$_GET["Lot_Mowed_TS"]."','".$_GET["Lot_State_ID"]."','".$_GET["Farmer_UUID"]."');");
		}
		else $missingFields = true;
	}
	else
	{
		$notFound = true;
		echo("Request not found! Request: ".$_GET['request']);
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
