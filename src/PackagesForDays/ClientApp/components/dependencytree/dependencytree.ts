import Vue from 'vue';
import { Component } from 'vue-property-decorator';

interface Result {
    input: string,
    output: string,
}

class Request {
    payload: string[];
    constructor() {
        this.payload = [];
    }
}

@Component
export default class FetchDataComponent extends Vue {
    results: Result[] = [];
    isLoading: boolean = false;
    graphDefinition: string = "";
 
    onSubmit() {
        this.isLoading = true;
        let request = new Request();
        var split = this.graphDefinition.split(',');
        for (var node = 0; node < split.length; node++) {

            var str = split[node].split('\'').join('');
            request.payload.push(str.trim());
        }

        fetch('api/DependencyChecker/CheckGraph', {
                method: 'post',
                headers: {
                    'Accept': 'application/json, text/plain, */*',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(request)
            }).then(res => res.json())
            .then(res => {
                this.isLoading = false;
                this.results.push(res);
            });
    }

    setValidExample() {
        this.graphDefinition = `'KittenService: CamelCaser', 'CamelCaser: '`;
    }

    setInvalidExample() {
        this.graphDefinition = `'KittenService: ','Leetmeme: Cyberportal','Cyberportal: Ice','CamelCaser: KittenService','Fraudstream: ','Ice: Leetmeme'`;

    }
}
