# ReadingSteiner
A counter/timer of sorts for Windows operating systems stylized to look like the divergence meter from Steins;Gate Viual Npvel
#What does that do exactly?
The app lets you set the time range that will be checked against the current time. If current time is exactly `startTime`, the returned value equals `0.000000`. When current time it's halfway through the range the returned value is `0.50000` and so on.
#Recent changes:
* Implemented /u/Hlug2340](https://www.reddit.com/user/Hlug2340)'s idea -- the meter realigns on leeft click/enter key press instead of random intervals;
* Added option to mute sound in `config.xml`;
* Gitted the source code;

#Quick notes
* Alt+F4 is also supposed to trigger realign (higher chance than Enter/Click) but I can't test that right now because it doesn't seem to work with LENOVO's function keys.

Feel free to use, modify and fork the code because I doubt I'll be updating it much