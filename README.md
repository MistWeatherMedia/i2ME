# MistWX-i2ME

![Discord](https://img.shields.io/discord/1059354045971693568)
![GitHub Release](https://img.shields.io/github/v/release/mewtek/mistwx-i2ME)
![GitHub License](https://img.shields.io/github/license/mewtek/mistwx-i2messageencoder)

i2ME is a compact data aggregator and distributor for The Weather Channel's IntelliStar 2 XD & IntelliStar 2 JR headend units.

For any support with this encoder, join our [Discord](https://discord.gg/hV2w5sZQxz), and we'll be more than happy to help out with any issues! Bug reports through this repo's [issues tab](https://github.com/mewtek/mistwx-i2messageencoder/issues) are also much appreciated, and helps narrow down what we need to fix.

**To get started, visit [our wiki!](https://github.com/mewtek/MistWX-i2ME/wiki/First%E2%80%90time-setup)**

## Features

i2ME handles data collection, record generation, as well as record sending through the unit's routine & priority message ports, closely simulating the way that real headends receive their data. On top of this, all files sent through this program are compressed through GZip, making files send much faster than the predecessor Python scripts.

Precipitation & Satellite radar images, while not finished, will be added in the near future.

## Data endpoints collected

- [x] Bulletins / Alerts
- [x] Current Observations
- [x] Daily Forecast
- [x] Hourly Forecast
- [x] Air Quality (currently broken, unknown)
- [x] Tide Forecast
- [x] Airport Delays (currently broken, i2 rejects it)
- [x] Pollen Forecast
- [x] Heating & Cooling
- [x] Aches & Pains
- [x] Breathing
- [x] Mosquito Activity
- [x] Climatology Record
- [x] Tropical Advisory
- [x] Pollen Observations
- [x] Mapping
- [x] Dry Skin
- [x] Location Info
- [x] Radar
- [x] Satellite Radar

**This list is subject to change.**

## Attributions

Powered by Copernicus Atmosphere Monitoring Service Information 2025

Neither the European Commission nor ECMWF is responsible for any use that may be made of the Copernicus Information or Data it contains.
