<?php
	include 'db_connect.php';

	$result = "";
	$notFound = false;
	$missingFields = false;

	if ($_GET['request'] == 'GetSheep')
	{
		if (isset($_GET["Sheep_UUID"]) and isset($_GET["Farmer_UUID"]))
		{   
		    $result = $mysqli->query("SELECT * FROM `VerweidklokSheepTable` WHERE Sheep_UUID = " 
		    	."'".$_GET["Sheep_UUID"]."' AND Farmer_UUID = '".$_GET["Farmer_UUID"]."';");
		}
		else $missingFields = true;
	}
	elseif ($_GET['request'] == 'GetAllSheep')
	{
		if (isset($_GET["Farmer_UUID"]))
		{
		    $result = $mysqli->query("SELECT * FROM `VerweidklokSheepTable` WHERE Farmer_UUID = '".$_GET["Farmer_UUID"]."';");
		}
		else $missingFields = true;
	}
	elseif ($_GET['request'] == 'GetLot')
	{
		if (isset($_GET["Lot_UUID"]) and isset($_GET["Farmer_UUID"]))
		{
		    $result = $mysqli->query("SELECT * FROM `VerweidklokLotTable` WHERE Lot_UUID = " 
		    	."'".$_GET["Lot_UUID"]."' AND Farmer_UUID = '".$_GET["Farmer_UUID"]."';");
		}
		else $missingFields = true;
	}
	elseif ($_GET['request'] == 'GetAllLot')
	{
		if (isset($_GET["Farmer_UUID"]))
		{
		    $result = $mysqli->query("SELECT * FROM `VerweidklokLotTable` WHERE Farmer_UUID = '".$_GET["Farmer_UUID"]."';");
		}
		else $missingFields = true;
	}
	elseif ($_GET['request'] == 'GetWorm')
	{
		if (isset($_GET["Worm_UUID"]))
		{
		    $result = $mysqli->query("SELECT * FROM `VerweidklokWormCollection` WHERE Lot_UUID = " 
		    	."'".$_GET["Lot_UUID"]."';");
		}
		else $missingFields = true;
	}
	elseif ($_GET['request'] == 'GetAllWorm')
	{
		$result = $mysqli->query("SELECT * FROM `VerweidklokWormCollection`;");
	}
	elseif ($_GET['request'] == 'GetPair')
	{
		if (isset($_GET["Pair_UUID"]) and isset($_GET["Farmer_UUID"]))
		{
		    $result = $mysqli->query("SELECT * FROM `VerweidklokPairCollection` WHERE Pair_DB_ID = " 
		    	."'".$_GET["Pair_UUID"]."' AND Farmer_UUID = '".$_GET["Farmer_UUID"]."';");
		}
		else $missingFields = true;
	}
	elseif ($_GET['request'] == 'GetAllPair')
	{
		if (isset($_GET["Farmer_UUID"]))
		{
		    $result = $mysqli->query("SELECT * FROM `VerweidklokPairCollection` WHERE Farmer_UUID = '".$_GET["Farmer_UUID"]."';");
		}
		else $missingFields = true;
	}
	elseif ($_GET['request'] == 'GetSheepWeight')
	{
		if (isset($_GET["Sheep_ID"]))
		{
		    $result = $mysqli->query("SELECT * FROM `VerweidklokSheepTable` WHERE Sheep_ID = " 
		    	."'".$_GET["Sheep_ID"]."';");
		}
		else $missingFields = true;
	}
	elseif ($_GET['request'] == 'GetSheepBreed')
	{
		if (isset($_GET["Sheep_ID"]))
		{
		    $result = $mysqli->query("SELECT * FROM `VerweidklokBreedTable` WHERE Sheep_ID = " 
		    	."'".$_GET["Sheep_ID"]."';");
		}
		else $missingFields = true;
	}
	elseif ($_GET['request'] == 'GetSheepFamily')
	{
		if (isset($_GET["Sheep_ID_1"]))
		{
		    $result = $mysqli->query("SELECT * FROM `VerweidklokSheepFamily` WHERE Sheep_ID_1 = " 
		    	."'".$_GET["Sheep_ID_1"]."';");
		}
		else $missingFields = true;
	}
	elseif ($_GET['request'] == 'GetSheepInfection')
	{
		if (isset($_GET["Sheep_ID"]))
		{
		    $result = $mysqli->query("SELECT * FROM `VerweidklokSheepInfection` WHERE Sheep_ID = " 
		    	."'".$_GET["Sheep_ID"]."';");
		}
		else $missingFields = true;
	}
	elseif ($_GET['request'] == 'GetSheepPair')
	{
		if (isset($_GET["Sheep_ID"]) and isset($_GET["Farmer_UUID"]))
		{
		    $result = $mysqli->query("SELECT * FROM `VerweidklokSheepPairs` WHERE Sheep_ID = " 
		    	."'".$_GET["Sheep_ID"]."' AND Farmer_UUID = '".$_GET["Farmer_UUID"]."';");
		}
		else $missingFields = true;
	}
	elseif ($_GET['request'] == 'GetSheepMedication')
	{
		if (isset($_GET["Sheep_ID"]) and isset($_GET["Farmer_UUID"]))
		{
		    $result = $mysqli->query("SELECT * FROM `VerweidklokSheepMedicine` WHERE Sheep_ID = " 
		    	."'".$_GET["Sheep_ID"]."' AND Farmer_UUID = '".$_GET["Farmer_UUID"]."';");
		}
		else $missingFields = true;
	}
	elseif ($_GET['request'] == 'VerweidklokWormResistences')
	{
		if (isset($_GET["Worm_ID"]))
		{
		    $result = $mysqli->query("SELECT * FROM `VerweidklokSheepTable` WHERE Worm_ID = " 
		    	."'".$_GET["Worm_ID"]."';");
		}
		else $missingFields = true;
	}
	elseif ($_GET['request'] == 'GetWormWeakness')
	{
		if (isset($_GET["Worm_ID"]))
		{
		    $result = $mysqli->query("SELECT * FROM `VerweidklokSheepTable` WHERE Worm_ID = " 
		    	."'".$_GET["Worm_ID"]."';");
		}
		else $missingFields = true;
	}
	elseif ($_GET['request'] == 'GetLotState')
	{
		if (isset($_GET["Lot_ID"]))
		{
		    $result = $mysqli->query("SELECT * FROM `VerweidklokLotStates` WHERE Lot_ID = " 
		    	."'".$_GET["Lot_ID"]."';");
		}
		else $missingFields = true;
	}
	elseif ($_GET['request'] == 'GetLotPlant')
	{
		if (isset($_GET["Lot_ID"]))
		{
		    $result = $mysqli->query("SELECT * FROM `VerweidklokLotPlants` WHERE Lot_ID = " 
		    	."'".$_GET["Lot_ID"]."';");
		}
		else $missingFields = true;
	}
	elseif ($_GET['request'] == 'GetLotLivestock')
	{
		if (isset($_GET["Lot_ID"]))
		{
		    $result = $mysqli->query("SELECT * FROM `VerweidklokLotAnimals` WHERE Lot_ID = " 
		    	."'".$_GET["Lot_ID"]."';");
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
