import React, {Component} from 'react';
import PropTypes from 'prop-types';

export class TopSellingAgentsTable extends Component {
  static displayName = TopSellingAgentsTable.name;

  constructor(props) {
    super(props);
    this.state = {topSellingAgents: [], error: undefined, loading: true};
  }

  static renderSpinner() {
    return (
      <div className="spinner-border" role="status">
        <span className="sr-only">Loading...</span>
      </div>
    )
  }

  static renderTable(topSellingAgents) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
        <tr>
          <th scope="col">#</th>
          <th scope="col">Id</th>
          <th scope="col">Name</th>
          <th scope="col">Object Count</th>
        </tr>
        </thead>
        <tbody>
        {topSellingAgents.map((agent, index) =>
          <tr key={agent.agentId}>
            <td>{index}</td>
            <td>{agent.agentId}</td>
            <td>{agent.agentName}</td>
            <td>{agent.objectCount}</td>
          </tr>
        )}
        </tbody>
      </table>
    );
  }

  static renderError(error) {
    return (
      <div className="alert alert-danger" role="alert">
        {error}
      </div>
    )
  }

  async componentDidMount() {
    await this.populateTopSellingAgents();
  }

  render() {
    let contents = this.state.loading
      ? TopSellingAgentsTable.renderSpinner()
      : this.state.error !== undefined
        ? TopSellingAgentsTable.renderError(this.state.error)
        : TopSellingAgentsTable.renderTable(this.state.topSellingAgents);

    return (
      <div>
        <h3 id="tabelLabel">{this.props.header}</h3>
        {contents}
      </div>
    );
  }

  async populateTopSellingAgents() {
    const response = await fetch(`api/v1/agents/top-sellers?searchQuery=${this.props.queryString}`);
    const responseParsed = await response.json();
    if (response.ok) {
      this.setState({topSellingAgents: responseParsed, error: undefined, loading: false});
    } else {
      const error = responseParsed.detail ? `${responseParsed.title} ${responseParsed.detail}` : responseParsed.title;
      this.setState({topSellingAgents: [], error: error, loading: false});
    }
  }
}

TopSellingAgentsTable.propTypes = {
  header: PropTypes.string,
  queryString: PropTypes.string
};
