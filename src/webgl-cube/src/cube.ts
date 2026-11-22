import * as THREE from 'three';
import Stats from 'stats.js';

export class CubeDemo {
    scene!: THREE.Scene;
    camera!: THREE.PerspectiveCamera;
    renderer!: THREE.WebGLRenderer;
    cube!: THREE.Mesh;
    ambientLight!: THREE.AmbientLight;
    stats!: Stats;
    loadCounter = 0;

    run() {
        this.prepareScene();
        this.render();
    }

    // the canvas fills the page it is on - in the site, an iframe whose size is only settled
    // once the host page's css applies, and that changes with the window.  so rather than read
    // the size once at startup, follow it: the observer reports the first real size and every
    // change after
    private followPageSize() {
        const resize = () => {
            const { clientWidth: width, clientHeight: height } = document.body;

            if (width === 0 || height === 0) {
                return;
            }

            // the canvas is sized by css (100%), so only the drawing buffer is set here
            this.renderer.setSize(width, height, false);
            this.camera.aspect = width / height;
            this.camera.updateProjectionMatrix();
        };

        new ResizeObserver(resize).observe(document.body);
    }

    private prepareScene() {
        // this.scene
        this.scene = new THREE.Scene();

        // fog
        this.scene.fog = new THREE.Fog(0x9999ff, 400, 1000);

        // renderer
        this.renderer = new THREE.WebGLRenderer({ antialias: true, alpha: true });
        this.renderer.setPixelRatio(window.devicePixelRatio);
        document.body.appendChild(this.renderer.domElement);

        // stats
        this.stats = new Stats();
        document.body.appendChild(this.stats.dom);

        // camera
        this.camera = new THREE.PerspectiveCamera(45, 1, 0.1, 1000);
        this.camera.position.set(0, 150, 400);
        this.camera.lookAt(this.scene.position);

        this.followPageSize();

        // ambient light
        this.ambientLight = new THREE.AmbientLight(0x404040);
        this.scene.add(this.ambientLight);

        // directional light
        const directionalLight = new THREE.DirectionalLight(0xffffff, 0.9);
        directionalLight.position.set(-1, 1, 1);
        directionalLight.castShadow = true;
        this.scene.add(directionalLight);

        // axis helper
        const axisHelper = new THREE.AxesHelper(100);
        this.scene.add(axisHelper);

        // cube
        const textureLoader = new THREE.TextureLoader();
        textureLoader.load('./DSC_8562.jpg', texture => {
            texture.colorSpace = THREE.SRGBColorSpace;
            const geometry = new THREE.BoxGeometry(50, 50, 50);
            const material = new THREE.MeshPhongMaterial({ color: 0xffffff, map: texture });
            this.cube = new THREE.Mesh(geometry, material);
            this.cube.position.set(0, 70, 180);
            this.scene.add(this.cube);

            this.loadCounter++;

            this.render();
        });

        // floor
        textureLoader.load('./floor_texture.jpg', texture => {
            texture.colorSpace = THREE.SRGBColorSpace;
            const floorPlane = new THREE.PlaneGeometry(1000, 1000);
            texture.wrapS = THREE.RepeatWrapping;
            texture.wrapT = THREE.RepeatWrapping;
            texture.repeat.set(9, 9);
            const floorMaterial = new THREE.MeshBasicMaterial({ map: texture }); //
            const floor = new THREE.Mesh(floorPlane, floorMaterial);
            floor.position.y = -30;
            floor.rotation.x = (Math.PI / 2) - .3;
            this.scene.add(floor);

            this.loadCounter++;

            this.render();
        });
    }

    private render() {
        if (this.loadCounter < 2) {
            return;
        }

        requestAnimationFrame(() => this.render());

        this.cube.rotation.x += 0.01;
        this.cube.rotation.y += 0.01;

        this.renderer.render(this.scene, this.camera);

        this.stats.update();
    }
}
