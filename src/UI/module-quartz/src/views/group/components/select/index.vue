<script>
import { mixins } from 'netmodular-ui'

const api = $api.quartz.group

export default {
  mixins: [mixins.select],
  data() {
    return {
      action: api.select
    }
  }
}
</script>
