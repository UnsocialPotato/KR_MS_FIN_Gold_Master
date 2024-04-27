<?php

$servername = "sim421chickenproject.000webhostapp.com";
$username = "id22059802_ripak1";
$password = "Pjr112792!!";
$dbname = "id22059802_chickenproject";

  // Create connection
  $conn = new mysqli($servername, $username, $password, $dbname);

  // Check connection
    if ($conn->connect_error) 
    {
        die("Connection failed: " . $conn->connect_error);
    }
  
    //variables submitted by user
    $itemID = $_POST["itemID"];

    $path = "http://localhost/fullstackproject/ItemsIcons/" . $itemID . ".png";

    //Get the image and convert into string
    $image = file_get_contents($path);

    echo $image;

    $conn ->close();
?>