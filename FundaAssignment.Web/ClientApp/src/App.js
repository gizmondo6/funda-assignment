import React, {Component} from 'react';
import {Layout} from './components/Layout';
import {TopSellingAgentsTable} from './components/TopSellingAgentsTable';


export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <Layout>
        <TopSellingAgentsTable queryString="/amsterdam/" header="10 top sellers /amsterdam/"/>
        <TopSellingAgentsTable queryString="/amsterdam/tuin/" header="10 top sellers /amsterdam/tuin/"/>
      </Layout>
    );
  }
}
