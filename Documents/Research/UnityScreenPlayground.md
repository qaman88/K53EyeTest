```c#

	public void computeCurrentOptotypeSize()
	{
		// scaling factors of the image rect transfrom
		Vector3 scale = this.imageOptotype.transform.localScale;

		// size of bound box - remains the same no matter the scale transform
		// canvas image scale with screen 100 pixels to 1 unit | image 118 * 118 
		Vector3 size = this.imageOptotype.GetComponent<Image>().sprite.bounds.size;
		float x =  scale.x * size.x; // 1.18 unit
		float y =  scale.y * size.y; // 1.18 unit
		float result = x * y;
		Console.WriteLine($"$$$ Image width {size.x} | height {size.y} | area: {result} | scaleX: {scale.x} | scaleY: {scale.y}");

		// screen size in pixels
		Vector2 screen = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
		Console.WriteLine($"$$$ Screen width: {screen.x} height: {screen.y}");

		// resolution (pixels) / print size (inches) = DPI (pixels per inch)
		float dpi = Screen.dpi;
		Vector2 print = screen / dpi;
		Console.WriteLine($"$$$ Screen inches width: {print.x} | height: {print.y} | DPI: {dpi}");

		// cm = ( pixels * 0.0254 ) / DPI
		float dpi2centmeter = 2.54f;
		Vector2 centmeters = new Vector2(screen.x * dpi2centmeter / dpi, screen.y * dpi2centmeter / dpi);
		Console.WriteLine($"$$$ Screen centmeters width: {centmeters.x} | height: {centmeters.y} | DPI: {dpi}");

		// m = ( pixels * 0.0254 ) / DPI
		float dpi2meter = 0.0254f;
		Vector2 meters = new Vector2(screen.x * dpi2meter / dpi, screen.y * dpi2meter / dpi);
		Console.WriteLine($"$$$ Screen meters width: {meters.x} | height: {meters.y} | DPI: {dpi}");

		// camera height and width
		// The minimal point of the box. This is always equal to center-extents.
		Vector3 min = this.imageOptotype.GetComponent<Image>().sprite.bounds.min;
		// The maximal point of the box. This is always equal to center+extents.
		Vector3 max = this.imageOptotype.GetComponent<Image>().sprite.bounds.max;
		Vector3 screenMin = camera.WorldToScreenPoint(min);
		Vector3 screenMax = camera.WorldToScreenPoint(max);
		// Screen space is defined in pixels.
		float screenWidth  = (screenMax.x - screenMin.x) * scale.x;
		float screenHeight = (screenMax.y - screenMin.y) * scale.y;
		Console.WriteLine($"$$$ Camera image width: {screenWidth} | height: {screenHeight}");

		// 1px = 0.026458 cm
		float px2cm = 0.026458f;
		Vector2 imageCentmeters = new Vector2(screenWidth * px2cm, screenHeight * px2cm);
		Console.WriteLine($"$$$ Image centmeters width: {imageCentmeters.x} | height: {imageCentmeters.y}");

		// 1px = 0.0002645833 m
		float px2m = 0.0002645833f;
		Vector2 imageMeters = new Vector2(screenWidth * px2m, screenHeight * px2m);
		Console.WriteLine($"$$$ Screen meters width: {imageMeters.x} | height: {imageMeters.y}");
	}
  ```