import * as THREE from 'three';
import Stats from 'stats.js';

import fragmentShader from './shader.frag?raw';
import vertexShader from './shader.vert?raw';

export class ShaderDemo {
    scene!: THREE.Scene;
    camera!: THREE.PerspectiveCamera;
    renderer!: THREE.WebGLRenderer;
    stats!: Stats;

    run() {
        this.prepareScene();
        this.render();
    }

    setupSphere() {
        const geometry = new THREE.SphereGeometry(100, 32, 32);

        const verts = geometry.getAttribute('position');
        const displacement = new Float32Array(verts.count);
        const colors = new Float32Array(verts.count * 3);
        const color = new THREE.Color();

        for (let v = 0, c = 0; v < verts.count; v++, c += 3) {
            displacement[v] = Math.random() * 20;

            color.setHSL(v / verts.count, 1.0, 0.5);

            colors[c + 0] = color.r;
            colors[c + 1] = color.g;
            colors[c + 2] = color.b;
        }

        geometry.setAttribute('displacement', new THREE.BufferAttribute(displacement, 1));
        geometry.setAttribute('colors', new THREE.BufferAttribute(colors, 3));

        const sphere = new THREE.Mesh(
            geometry,
            new THREE.ShaderMaterial({
                vertexShader: vertexShader,
                fragmentShader: fragmentShader
            })
        );

        this.scene.add(sphere);
    }

    render() {
        // requestAnimationFrame(() => this.render());

        this.renderer.render(this.scene, this.camera);

        this.stats.update();
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

            // the scene is drawn once rather than every frame, so draw it again at the new size
            this.render();
        };

        new ResizeObserver(resize).observe(document.body);
    }

    private prepareScene() {
        this.scene = new THREE.Scene();

        this.renderer = new THREE.WebGLRenderer({ antialias: true, alpha: true });
        this.renderer.setPixelRatio(window.devicePixelRatio);
        document.body.appendChild(this.renderer.domElement);

        this.stats = new Stats();
        document.body.appendChild(this.stats.dom);

        this.camera = new THREE.PerspectiveCamera(45, 1, 0.1, 10000);
        // far enough back that the spikes (up to 20 past the sphere's 100) clear the frame's edges
        this.camera.position.set(0, 0, 380);
        this.camera.lookAt(this.scene.position);

        this.setupSphere();
        this.followPageSize();
    }
}
