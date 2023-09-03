# BadAppleSkulls
The plugin I used to create Bad Apple video on skulls

### Important
This instruction is extremely concise: I'm not going into details about compiling and installing BepInEx plugins, assuming you know how to do it on your own.

In order to recreate the video:
1. Choose a base resolution for your video (I used 180x135 resolution in the video, but it severly impacts performance, so you're at liberty to alter it). Keep in mind that Bad Apple has 4:3 aspect ratio.
2. Get to your root ULTRAKILL folder and create a folder named `badapple`.
3. Place source Bad Apple mp4 named `badapple.mp4` into `badapple` folder.
4. Within this folder, convert Bad Apple to a series of PNG images of chosen resolution using `ffmpeg`. Let's say your choise is 60x45, so the final ffmpeg command is:
   ```
   ffmpeg -i badapple.mp4 -vf "scale=60:45" output_%04d.png
   ```
5. Delete `badapple.mp4` file so `badapple` folder would only contain .png files.
6. In the source file `BadAppleScreenController.cs`, change `_screenWidth` and `_screenHeight` constants according to the resolution of your choise.
7. Compile and install the plugin.
8. Launch ULTRAKILL and get to the sandbox.

Now, if everything's been done correctly, ULTRAKILL console should have two new commands:

`bad_apple_screen`: Places four corner sculls of your screen. Please note that you must have spawner arm selected before entering this command. These sculls will get you the idea where your screen is going to be so you can align the camera

`bad_apple_play`: Spawns all the sculls and starts playing Bad Apple while making screenshots. Resulting screenshots are saved inside `badapple-rendered` folder in your root ULTRAKILL folder.
