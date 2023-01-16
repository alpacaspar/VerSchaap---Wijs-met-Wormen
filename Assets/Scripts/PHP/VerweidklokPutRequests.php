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
	elseif ($_GET["request"] == 'UpdateLot')
	{
		if (isset($_GET["Lot_UUID"]) and isset($_GET["Farmer_UUID"]))
		{
		    $result = $mysqli->query("UPDATE `VerweidklokLotTable` SET Lot_Name = '".$_GET["Lot_Name"]."', Lot_Surface = '".$_GET["Lot_Surface"]."', Lot_Quality = '".$_GET["Lot_Quality"]."', Lot_Mowed_TS = '".$_GET["Lot_Mowed_TS"]."', Lot_State_ID = '".$_GET["Lot_State_ID"]."'"
				." WHERE Lot_UUID = '".$_GET["Lot_UUID"]."' AND Farmer_UUID = '".$_GET["Farmer_UUID"]."';");
		}
		else $missingFields = true;
	}
	elseif ($_GET["request"] == 'UpdateSheepPair')
	{
		if (isset($_GET["Pair_DB_ID"]) and isset($_GET["Farmer_UUID"]))
		{
		    $result = $mysqli->query("UPDATE `VerweidklokPairCollection` SET Pair_Name = '".$_GET["Pair_Name"]."', TS_Formed = '".$_GET["TS_Formed"]."', TS_Removed = '".$_GET["TS_Removed"]."'"
				." WHERE Pair_DB_ID = '".$_GET["Pair_DB_ID"]."' AND Farmer_UUID = '".$_GET["Farmer_UUID"]."';");
		}
		else $missingFields = true;
	}
	elseif ($_GET["request"] == 'UpdateSheepWeight')
	{
		if (isset($_GET["Weight_DB_ID"]) and isset($_GET["Farmer_UUID"]))
		{
		    $result = $mysqli->query("UPDATE `VerweidKlokWeightTable` SET Sheep_ID = '".$_GET["Sheep_ID"]."', Weight = '".$_GET["Weight"]."'"
				." WHERE Weight_DB_ID = '".$_GET["Weight_DB_ID"]."' AND Farmer_UUID = '".$_GET["Farmer_UUID"]."';");
		}
		else $missingFields = true;
	}
	elseif ($_GET["request"] == 'UpdateSheepBreed')
	{
		if (isset($_GET["Breed_DB_ID"]))
		{
		    $result = $mysqli->query("UPDATE `VerweidklokBreedTable` SET Sheep_ID = '".$_GET["Sheep_ID"]."', Breed_ID = '".$_GET["Breed_ID"]."'"
				." WHERE Breed_DB_ID = '".$_GET["Breed_DB_ID"]."';");
		}
		else $missingFields = true;
	}
	elseif ($_GET["request"] == 'UpdateSheepFamily')
	{
		if (isset($_GET["Family_DB_ID"]))
		{
		    $result = $mysqli->query("UPDATE `VerweidklokSheepFamily` SET Sheep_ID_1 = '".$_GET["Sheep_ID_1"]."', Sheep_ID_2 = '".$_GET["Sheep_ID_2"]."', Family_Type_ID = '".$_GET["Family_Type_ID"]."'"
				." WHERE Family_DB_ID = '".$_GET["Family_DB_ID"]."' AND Farmer_UUID = '".$_GET["Farmer_UUID"]."';");
		}
		else $missingFields = true;
	}
	elseif ($_GET["request"] == 'UpdatePair')
	{
		if (isset($_GET["Sheep_Pair_DB_ID"]) and isset($_GET["Farmer_UUID"]))
		{
		    $result = $mysqli->query("UPDATE `VerweidklokSheepPairs` SET Sheep_ID = '".$_GET["Sheep_ID"]."', Pair_ID = '".$_GET["Pair_ID"]."'"
				." WHERE Sheep_Pair_DB_ID = '".$_GET["Sheep_Pair_DB_ID"]."' AND Farmer_UUID = '".$_GET["Farmer_UUID"]."';");
		}
		else $missingFields = true;
	}
	elseif ($_GET["request"] == 'UpdateSheepInfection')
	{
		if (isset($_GET["Infection_DB_ID"]))
		{
		    $result = $mysqli->query("UPDATE `VerweidklokSheepInfection` SET Sheep_ID = '".$_GET["Sheep_ID"]."', Infection_ID = '".$_GET["Infection_ID"]."'"
				." WHERE Infection_DB_ID = '".$_GET["Infection_DB_ID"]."';");
		}
		else $missingFields = true;
	}
	elseif ($_GET["request"] == 'UpdateSheepMedication')
	{
		if (isset($_GET["Sheep_ID"]) and isset($_GET["Farmer_UUID"]))
		{
		    $result = $mysqli->query("UPDATE `VerweidklokSheepMedicine` SET Medicine_ID = '".$_GET["Medicine_ID"]."', Inject_Timestamp = '".$_GET["Inject_Timestamp"]."', Dosage = '".$_GET["Dosage"]."'"
				." WHERE Sheep_ID = '".$_GET["Sheep_ID"]."' AND Farmer_UUID = '".$_GET["Farmer_UUID"]."';");
		}
		else $missingFields = true;
	}
	elseif ($_GET["request"] == 'UpdateWorm')
	{
		if (isset($_GET["Worm_UUID"]))
		{
		    $result = $mysqli->query("UPDATE `VerweidklokWormCollection` SET Worm_Latin_Name = '".$_GET["Worm_Latin_Name"]."', Worm_Normal_Name = '".$_GET["Worm_Normal_Name"]."', Worm_EPG_Danger = '".$_GET["Worm_EPG_Danger"]."', Worm_Egg_Description = '".$_GET["Worm_Egg_Description"]."'"
				." WHERE Worm_UUID = '".$_GET["Worm_UUID"]."';");
		}
		else $missingFields = true;
	}
	elseif ($_GET["request"] == 'UpdateWormResistence')
	{
		if (isset($_GET["Worm_Res_DB_ID"]))
		{
		    $result = $mysqli->query("UPDATE `VerweidklokWormResistences` SET Worm_ID = '".$_GET["Worm_ID"]."', Medicine_ID = '".$_GET["Medicine_ID"]."', Resistence_Percentage = '".$_GET["Resistence_Percentage"]."'"
				." WHERE Worm_Res_DB_ID = '".$_GET["Worm_Res_DB_ID"]."';");
		}
		else $missingFields = true;
	}
	elseif ($_GET["request"] == 'UpdateWormWeakness')
	{
		if (isset($_GET["Worm_Weak_DB_ID"]))
		{
		    $result = $mysqli->query("UPDATE `VerweidklokWormWeakness` SET Worm_ID = '".$_GET["Worm_ID"]."', Medicine_ID = '".$_GET["Medicine_ID"]."', Effective_Percentage = '".$_GET["Effective_Percentage"]."'"
				." WHERE Worm_Weak_DB_ID = '".$_GET["Worm_Weak_DB_ID"]."';");
		}
		else $missingFields = true;
	}
	elseif ($_GET["request"] == 'UpdateLotPlant')
	{
		if (isset($_GET["Lot_Plant_DB_ID"]))
		{
		    $result = $mysqli->query("UPDATE `VerweidklokLotPlants` SET Lot_ID = '".$_GET["Lot_ID"]."', Plant_ID = '".$_GET["Plant_ID"]."'"
				." WHERE Lot_Plant_DB_ID = '".$_GET["Lot_Plant_DB_ID"]."';");
		}
		else $missingFields = true;
	}

	elseif ($_GET["request"] == 'UpdateLotLivestock')
	{
		if (isset($_GET["Lot_Animal_DB_ID"]))
		{
		    $result = $mysqli->query("UPDATE `VerweidklokLotAnimals` SET Lot_ID = '".$_GET["Lot_ID"]."', Livestock_ID = '".$_GET["Livestock_ID"]."', Lot_Animal_TS = '".$_GET["Lot_Animal_TS"]."'"
				." WHERE Lot_Animal_DB_ID = '".$_GET["Lot_Animal_DB_ID"]."';");
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
