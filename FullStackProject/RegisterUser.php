<?php

$servername = "sim421chickenproject.000webhostapp.com";
$username = "id22059802_ripak1";
$password = "Pjr112792!!";
$dbname = "id22059802_chickenproject";

//variables submitted by user
$loginUser = $_POST["loginUser"];
$loginPass = $_POST["loginPass"];

// Create connection
$conn = new mysqli($servername, $username, $password, $dbname);

// Check connection
  if ($conn->connect_error) 
  {
      die("Connection failed: " . $conn->connect_error);
  }

//echo "Connected successfully, now we will show the users.". "<br><br>";

$sql = "SELECT username FROM users WHERE username = '". $loginUser. "'";

$result = $conn->query($sql);

  if ($result->num_rows > 0) 
  {
      //Tell user that the name is already taken
      echo "Username is already taken"; 
      
  } else 
      {
          echo "Creating user..."; 

          //Insert the user and password into the database
          $sql2 = "INSERT INTO users (username, password, level, coins) VALUES ('". $loginUser. "', '". $loginPass. "', 1, 0)";

          if ($conn->query($sql2) === TRUE) 
            {
              echo "New record created successfully";
            } 
            else 
            {
              echo "Error: " . $sql2 . "<br>" . $conn->error;
            }
      }

$conn->close();

?>